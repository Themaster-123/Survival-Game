using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[AddComponentMenu("Survival Game/World/World")]
public class World : MonoBehaviour
{
    public static Vector3Int[] closestChunks = null; 

    public WorldSettings worldSettings;
    public NoiseSettings noiseSettings;
    public GameObject chunkPrefab;

	[SerializeField]
	protected string nodeTree;
	protected Thread worldLoadThread;
	protected Dictionary<Vector3Int, ChunkData> chunkDataList;
	protected Vector3Int[] playerChunkPositions;
	protected FastNoise noise;

	public List<Player> Players { get; protected set; }
	public List<Entity> Entities { get; protected set; }

	protected Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

	public virtual void AddPlayer(Player player)
	{
		Players.Add(player);
	}

	public virtual void RemovePlayer(Player player)
	{
		Players.Remove(player);

	}

	public virtual void AddEntity(Entity entity)
	{
		Entities.Add(entity);
	}

	public virtual void RemoveEntity(Entity entity)
	{
		Entities.Remove(entity);
	}

	public virtual void LoadChunk(Vector3Int chunkPosition)
	{
        if (!IsChunkLoaded(chunkPosition))
		{
			Chunk chunk = CreateChunk(chunkPosition);
			chunk.InitiateChunk(chunkPosition, this);
			lock (chunks)
			{
				chunks.Add(chunkPosition, chunk);
			}
		}
	}

	public virtual void LoadChunk(Vector3Int chunkPosition, in Voxel[] voxels, in Vector3[] vertices, in int[] triangles)
	{
		if (!IsChunkLoaded(chunkPosition))
		{
			Chunk chunk = CreateChunk(chunkPosition);
			chunk.InitiateChunk(chunkPosition, voxels, vertices, triangles, this);
			lock (chunks)
			{
				chunks.Add(chunkPosition, chunk);
			}
		}
	}

	public virtual void UnloadChunk(Vector3Int chunkPosition)
	{
		if (IsChunkLoaded(chunkPosition))
		{
			Destroy(chunks[chunkPosition].gameObject);
			lock (chunks)
			{
				chunks.Remove(chunkPosition);
			}
		}
	}

	public virtual Chunk GetChunk(Vector3Int chunkposition)
	{
		lock (chunks)
		{
			if (IsChunkLoaded(chunkposition))
			{
				return chunks[chunkposition];
			}
		}

		return null;
	}

	public virtual bool IsChunkLoaded(Vector3Int chunkPosition)
	{
		lock (chunks)
		{
			return chunks.ContainsKey(chunkPosition);
		}
	}

	public virtual void SetVoxel(Voxel voxel, Vector3Int pos)
	{
		Vector3Int chunkPos = ChunkPositionUtilities.ToChunkPosition(pos, this);
		if (IsChunkLoaded(chunkPos))
		{
			chunks[chunkPos].SetVoxel(GetLocalVoxelPos(pos), voxel);
		}
	}

	public virtual Voxel GetVoxel(Vector3Int pos)
	{
		Vector3Int chunkPos = ChunkPositionUtilities.ToChunkPosition(pos, this);
		return GetVoxelAtChunk(GetLocalVoxelPos(pos), chunkPos);
	}

	public virtual Voxel GetVoxelAtChunk(Vector3Int pos, Vector3Int chunkPos)
	{
		if (IsChunkLoaded(chunkPos))
		{
			return chunks[chunkPos].GetVoxel(pos);
		}

		return new Voxel();
	}

	public virtual Vector3Int GetLocalVoxelPos(Vector3Int position)
	{
		position.x %= worldSettings.ChunkResolution;
		position.y %= worldSettings.ChunkResolution;
		position.z %= worldSettings.ChunkResolution;

		return position;
	}

	protected virtual void Awake()
	{
		InitializeLists();
		CreateNoise();
	}

	protected virtual void Update()
	{
		//LoadChunksNearPlayers();
		UpdatePlayerChunkPosition();
		GetChunksFromChunkData();
		UnloadFarAwayChunks();
	}

	protected virtual void OnEnable()
	{
		StartWorldLoadLoop();
	}

	protected virtual void OnDisable()
	{
		DestroyWorldLoadLoop();
	}

	protected virtual void OnValidate()
	{
        noiseSettings.resolution = worldSettings.ChunkResolution;
	}


	protected virtual void StartWorldLoadLoop()
	{
		worldLoadThread = new Thread(new ThreadStart(WorldLoadLoop));
		worldLoadThread.Name = "WORLD THREAD GO BRRR";
		worldLoadThread.Start();
	}

	protected virtual void DestroyWorldLoadLoop()
	{
		worldLoadThread.Abort();
	}

	// loops through all players and loads chunks
	protected virtual void LoadChunksNearPlayers()
	{
		foreach (Player player in Players)
		{
			LoadClosestUnloadedChunk(player.GetChunkPosition());
		}
	}

	// loads the closest unloaded Chunk.
	protected virtual void LoadClosestUnloadedChunk(Vector3Int position)
	{
		if (closestChunks == null)
		{
			closestChunks = GetClosestChunks(Settings.Instance.ChunkLoadDistance);
		}

		for (int i = 0; i < closestChunks.Length; i++)
		{
			Vector3Int chunkPosition = closestChunks[i] + position;
			if (!IsChunkLoaded(chunkPosition))
			{
				LoadChunk(chunkPosition);
				break;
			}
		}
	}

	protected virtual void GetChunksFromChunkData()
	{
		lock (chunkDataList)
		{
			foreach (KeyValuePair<Vector3Int, ChunkData> data in chunkDataList)
			{
				LoadChunk(data.Value.position, data.Value.voxels, data.Value.vertices, data.Value.triangles);
			}
			chunkDataList.Clear();
		}
	}

	protected virtual void InitializeLists()
	{
		Players = new List<Player>();
		Entities = new List<Entity>();
		chunkDataList = new Dictionary<Vector3Int, ChunkData>();
		playerChunkPositions = new Vector3Int[0];
	}

	protected virtual void CreateNoise()
	{
		noise = FastNoise.FromEncodedNodeTree(nodeTree);
	}

	protected virtual Chunk CreateChunk(Vector3Int chunkPosition)
	{
		GameObject chunkObject = Instantiate(chunkPrefab);
		chunkObject.transform.parent = transform;
		chunkObject.name = chunkPosition.ToString();
		return chunkObject.GetComponent<Chunk>();
	}

	protected virtual void UpdatePlayerChunkPosition()
	{
		lock (playerChunkPositions)
		{
			playerChunkPositions = new Vector3Int[Players.Count];
			for (int i = 0; i < Players.Count; i++)
			{
				playerChunkPositions[i] = Players[i].GetChunkPosition();
			}
		}
	}

	protected virtual void UnloadFarAwayChunks()
	{
		List<Vector3Int> chunksToUnload = new List<Vector3Int>();

		foreach (Vector3Int chunkPos in chunks.Keys) {
			uint minDistance = uint.MaxValue;

			foreach (Player player in Players)
			{
				uint tempDistance = ChunkPositionUtilities.Distance(player.GetChunkPosition(), chunkPos);
				if (minDistance > tempDistance)
				{
					minDistance = tempDistance;
				}
			}

			if (minDistance > Settings.Instance.ChunkLoadDistance)
			{
				chunksToUnload.Add(chunkPos);
			}
		}

		for (int i = 0; i < chunksToUnload.Count; i++)
		{
			UnloadChunk(chunksToUnload[i]);
		}
	}

	// gets closest chunk to load
	public static Vector3Int[] GetClosestChunks(uint chunkLoadDistance)
	{
		uint realSize = (chunkLoadDistance * 2) + 1;
		uint sizeSqrd = (realSize * realSize);
		uint sizeCubed = (sizeSqrd * realSize);

		Vector3Int[] traversedChunks = new Vector3Int[sizeCubed];

		Vector3Int offsetPos = new Vector3Int(0, 0, 0);
		Vector2Int direction = new Vector2Int(0, -1);
		int moveAmount = 1;

		for (int i = 0, increase = 0, amountMoved = 0, traversedIndex = 0; i < sizeSqrd; i++, amountMoved++, traversedIndex++)
		{
			if (increase == 2)
			{
				moveAmount++;
				increase = 0;
			}

			traversedChunks[traversedIndex] = offsetPos;

			for (int j = 1; j <= chunkLoadDistance; j++)
			{
				traversedIndex++;
				traversedChunks[traversedIndex] = offsetPos + new Vector3Int(0, -j, 0);
			}
			for (int j = 1; j <= chunkLoadDistance; j++)
			{
				traversedIndex++;
				traversedChunks[traversedIndex] = offsetPos + new Vector3Int(0, j, 0);
			}

			if (amountMoved == moveAmount)
			{
				amountMoved = 0;
				int oldDirX = direction.x;
				direction.x = -direction.y;
				direction.y = oldDirX;
				increase++;
			}

			offsetPos.x += direction.x;
			offsetPos.z += direction.y;
		}

		return traversedChunks;

	}

	// multithreaded world loading loop
	protected void WorldLoadLoop()
	{
		uint prevSize = 0;
		Vector3Int[] closestChunks = new Vector3Int[0];

		while (true)
		{
			uint size = Settings.Instance.GetChunkLoadDistance();


			if (prevSize != size)
			{
				prevSize = size;
				closestChunks = GetClosestChunks(size);
			}

			int playerSize;
			lock (playerChunkPositions)
			{
				playerSize = playerChunkPositions.Length;
			}

			for (int j = 0; j < playerSize; j++)
			{
				Vector3Int playerChunksPos;
				lock (playerChunkPositions)
				{
					if (j >= playerChunkPositions.Length)
					{
						break;
					}
					playerChunksPos = playerChunkPositions[j];
				}
				for (int i = 0; i < closestChunks.Length; i++)
				{
					Vector3Int chunkPosition = closestChunks[i] + playerChunksPos;

					bool loaded = false;

					lock (chunkDataList)
					{
						loaded = IsChunkLoaded(chunkPosition) || chunkDataList.ContainsKey(chunkPosition);
					}

					if (!loaded)
					{
						ChunkData chunkData = new ChunkData();
						NoiseSettings settings = noiseSettings;
						settings.offset += chunkPosition * worldSettings.ChunkResolution;

						float[] voxelData = new float[settings.resolution * settings.resolution * settings.resolution];
						FastNoise.OutputMinMax minMax = noise.GenUniformGrid3D(voxelData, (int)settings.offset.x, (int)settings.offset.y, (int)settings.offset.z, settings.resolution, settings.resolution, settings.resolution, settings.size.x, (int)settings.seed);

						chunkData.position = chunkPosition;

						chunkData.voxels = Chunk.GetVoxelsFromNoiseData(voxelData, worldSettings, Vector3Int.one * worldSettings.ChunkResolution);

						MarchingCubes.GenerateMesh(Vector3Int.one * (worldSettings.ChunkResolution), 0, chunkData.voxels, out chunkData.vertices, out chunkData.triangles);

						lock (chunkDataList)
						{
							chunkDataList.Add(chunkPosition, chunkData);
						}
						break;
					}
				}
			}
		}
	}

	[BurstCompile(CompileSynchronously = true)]
	protected struct WorldGenerationJob : IJob
	{
		[WriteOnly] NativeHashMap<int, ChunkData> chunks;
		public void Execute()
		{
		}
	}
}
