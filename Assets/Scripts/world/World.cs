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

	protected Thread worldLoadThread;
	protected Mutex playerListMutex;
	protected List<ChunkData> chunkData;

	public List<Player> Players { get; protected set; }
	public List<Entity> Entities { get; protected set; }

	protected Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

	public void AddPlayer(Player player)
	{
		lock(Players)
		{
			Players.Add(player);
		}
	}

	public void RemovePlayer(Player player)
	{
		lock (Players)
		{
			Players.Remove(player);
		}
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

    public bool IsChunkLoaded(Vector3Int chunkPosition)
	{
        return chunks.ContainsKey(chunkPosition);
	}

	protected virtual void Awake()
	{
		InitializeLists();
	}

	protected virtual void Update()
	{
		LoadChunksNearPlayers();
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
        noiseSettings.resolution = worldSettings.ChunkResolution + 1;
	}

	protected virtual void StartWorldLoadLoop()
	{
		worldLoadThread = new Thread(new ThreadStart(() => WorldLoadLoop()));
		worldLoadThread.Start();
		worldLoadThread.Name = "WORLD THREAD GO BRRR";
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

	// multithreaded world loading loop
	protected void WorldLoadLoop()
	{
		uint oldSize = Settings.Instance.GetChunkLoadDistance();
		int playerSize;
		lock (Players)
		{
			playerSize = Players.Count;
		}
		Vector3Int[] closestChunks = GetClosestChunks(oldSize);

		for (int j = 0; j < playerSize; j++)
		{
			Vector3Int playerChunksPos;
			lock(Players)
			{
				if (j >= Players.Count)
				{
					break;
				}
				playerChunksPos = Players[j].GetChunkPosition();
			}
			for (int i = 0; i < closestChunks.Length; i++)
			{

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
