using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Profiling;

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

	public virtual void SetVoxel(in Voxel voxel, Vector3Int pos)
	{
		Vector3Int chunkPos = ChunkPositionUtilities.VoxelToChunkPosition(pos, this);
		if (IsChunkLoaded(chunkPos))
		{
			Vector3Int localPos = GetLocalVoxelPos(pos);
			Vector3Int direction = Vector3Int.zero;
			for (int i = 0; i < 3; i++)
			{
				int dir = localPos[i] == 0 ? -1 : (localPos[i] == worldSettings.ChunkResolution - 1 ? 1 : 0);
				direction[i] = dir;
			}

			for (int x = Mathf.Min(direction.x, 0); x <= Mathf.Max(direction.x, 0); x++)
			{
				for (int y = Mathf.Min(direction.y, 0); y <= Mathf.Max(direction.y, 0); y++)
				{
					for (int z = Mathf.Min(direction.z, 0); z <= Mathf.Max(direction.z, 0); z++)
					{

						Vector3Int localNeighborPos = new Vector3Int(x, y, z);
						if (localNeighborPos != Vector3Int.zero)
						{
							Chunk chunk = GetChunk(chunkPos + localNeighborPos);
							if (chunk != null)
							{
								chunk.ReloadModel();
							}
						}
					}
				}
			}
			


			chunks[chunkPos].SetVoxel(localPos, voxel);
		}
	}

	public virtual void AddVoxel(in Voxel voxel, Vector3Int pos)
	{
		Voxel prevVoxel = GetVoxel(pos);
		Voxel newVoxel = voxel;
		newVoxel.value = prevVoxel.value + newVoxel.value;
		SetVoxel(newVoxel, pos);
	}

	public virtual Voxel GetVoxel(Vector3Int pos)
	{
		Vector3Int chunkPos = ChunkPositionUtilities.VoxelToChunkPosition(pos, this);
		return GetVoxelAtChunk(GetLocalVoxelPos(pos), chunkPos);
	}

	public virtual Voxel GetVoxelAtChunk(Vector3Int pos, Vector3Int chunkPos)
	{
		lock (chunks)
		{
			if (IsChunkLoaded(chunkPos))
			{
				return chunks[chunkPos].GetVoxel(pos);
			}
		}

		float[] data = new float[1];

		GenerateNoise(data, (Vector3Int.FloorToInt(noiseSettings.offset) + chunkPos * noiseSettings.resolution) + pos, Vector3Int.one);

		return new Voxel((Vector3)pos / worldSettings.ChunkResolution * worldSettings.ChunkSize, VoxelUtilities.ClampVoxelValue(-data[0]));
	}

	public virtual Vector3Int GetLocalVoxelPos(Vector3Int position)
	{
		position.x = MathUtilities.Mod(position.x, worldSettings.ChunkResolution);
		position.y = MathUtilities.Mod(position.y, worldSettings.ChunkResolution);
		position.z = MathUtilities.Mod(position.z, worldSettings.ChunkResolution);

		return position;
	}

	public virtual FastNoise.OutputMinMax GenerateNoise(in float[] voxelData, Vector3Int offset, Vector3Int resolution)
	{
		return noise.GenUniformGrid3D(voxelData, offset.x, offset.y, offset.z, resolution.x, resolution.y, resolution.z, noiseSettings.size.x, (int)noiseSettings.seed);
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

/*	protected virtual bool ChangeVoxel(in Voxel voxel, in Vector3Int pos)
	{
		Vector3Int chunkPos = ChunkPositionUtilities.VoxelToChunkPosition(pos, this);
		if (IsChunkLoaded(chunkPos))
		{
			Vector3Int localPos = GetLocalVoxelPos(pos);

			for (int i = 0; i < 3; i++)
			{
				int dir = localPos[i] == 0 ? -1 : (localPos[i] == worldSettings.ChunkResolution - 1 ? 1 : 0);
				if (dir != 0)
				{
					Vector3Int direction = Vector3Int.zero;
					direction[i] = dir;
					Chunk chunk = GetChunk(chunkPos + direction);
					if (chunk != null)
					{
						chunk.ReloadModel();
					}
				}
			}

			chunks[chunkPos].SetVoxel(localPos, voxel);
		}
	}*/

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
						Vector3Int offset = Vector3Int.FloorToInt(noiseSettings.offset + (chunkPosition * worldSettings.ChunkResolution));

						float[] voxelData = new float[noiseSettings.resolution * noiseSettings.resolution * noiseSettings.resolution];
						FastNoise.OutputMinMax minMax = GenerateNoise(voxelData, offset, Vector3Int.one * noiseSettings.resolution);

						chunkData.position = chunkPosition;

						chunkData.voxels = Chunk.GetVoxelsFromNoiseData(voxelData, worldSettings, Vector3Int.one * worldSettings.ChunkResolution);

						MarchingCubes.GenerateMesh(Vector3Int.one * (worldSettings.ChunkResolution), 0, chunkData.voxels, out chunkData.vertices, out chunkData.triangles, chunkPosition, this);

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
}
