using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

	public List<Player> Players { get; protected set; }
	public List<Entity> Entities { get; protected set; }

	protected Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

	public void AddPlayer(Player player)
	{
		Players.Add(player);
	}

	public void RemovePlayer(Player player)
	{
		Players.Remove(player);
	}

	public void AddEntity(Entity entity)
	{
		Entities.Add(entity);
	}

	public void RemoveEntity(Entity entity)
	{
		Entities.Remove(entity);
	}

	public void LoadChunk(Vector3Int chunkPosition)
	{
        if (!IsChunkLoaded(chunkPosition))
		{
            GameObject chunkObject = Instantiate(chunkPrefab);
            chunkObject.transform.parent = transform;
            chunkObject.name = chunkPosition.ToString();
            Chunk chunk = chunkObject.GetComponent<Chunk>();
            chunk.InitiateChunk(chunkPosition, this);
            chunks.Add(chunkPosition, chunk);
		}
	} 

	public void LoadChunk(Vector3Int chunkPosition, Voxel[] voxels, Vector3[] vertices, int[] triangles)
	{

	}

    public bool IsChunkLoaded(Vector3Int chunkPosition)
	{
        return chunks.ContainsKey(chunkPosition);
	}

	protected virtual void Awake()
	{
		InitializeLists();
	}

    // Start is called before the first frame update
	protected virtual void Update()
	{
		LoadChunksNearPlayers();
	}

	protected virtual void OnValidate()
	{
        noiseSettings.resolution = worldSettings.ChunkResolution + 1;
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

	protected virtual void InitializeLists()
	{
		Players = new List<Player>();
		Entities = new List<Entity>();
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

	//[BurstCompile(CompileSynchronously = true)]
	protected struct WorldChunkStripJob : IJobParallelFor
	{
		public WorldSettings worldSettings;
		public NoiseSettings noiseSettings;
		[WriteOnly] public NativeHashMap<Vector3, ChunkData> chunks;
		[ReadOnly] public NativeArray<Vector3Int> chunksToLoad;

		public void Execute(int i)
		{
			ChunkData chunkData = new ChunkData();
			NoiseSettings settings = noiseSettings;
			settings.offset += (Vector3)chunksToLoad[i] * worldSettings.ChunkResolution;

			float[] voxelData = GpuNoise.GenerateNoise(settings);

			chunkData.voxels = UpdateVoxels(voxelData);

			MarchingCubes.GenerateMesh(Vector3Int.one * (worldSettings.ChunkResolution + 1), .5f, chunkData.voxels, out chunkData.vertices, out chunkData.triangles);

			chunks.Add(chunksToLoad[i], chunkData);
		}

		private Voxel[] UpdateVoxels(in float[] voxelData)
		{
			Voxel[] voxels = new Voxel[voxelData.Length];

			// i do this to fix the seam problem
			int increasedResolution = worldSettings.ChunkResolution + 1;

			for (int x = 0; x < increasedResolution; x++)
			{
				for (int y = 0; y < increasedResolution; y++)
				{
					for (int z = 0; z < increasedResolution; z++)
					{
						int index = x + increasedResolution * (y + increasedResolution * z);
						Voxel voxel;
						voxel.position = new Vector3(x, y, z) / worldSettings.ChunkResolution * worldSettings.ChunkSize;
						voxel.value = voxelData[index];
						voxels[index] = voxel;
					}
				}
			}

			return voxels;
		}
	}
}
