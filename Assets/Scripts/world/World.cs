using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Survival Game/World/World")]
public class World : MonoBehaviour
{
    public WorldSettings worldSettings;
    public NoiseSettings noiseSettings;
    public GameObject chunkPrefab;

    protected Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

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

    // Start is called before the first frame update
    protected void Start()
    {
        LoadChunk(new Vector3Int(0, 0, 0));
        LoadChunk(new Vector3Int(1, 0, 0));
        LoadChunk(new Vector3Int(0, 0, 1));
        LoadChunk(new Vector3Int(1, 0, 1));
        LoadChunk(new Vector3Int(2, 0, 2));
        LoadChunk(new Vector3Int(3, 0, 2));
        LoadChunk(new Vector3Int(2, 0, 3));
        LoadChunk(new Vector3Int(3, 0, 2));
        LoadChunk(new Vector3Int(4, 0, 2));
        LoadChunk(new Vector3Int(5, 0, 4));
        LoadChunk(new Vector3Int(4, 0, 5));
        LoadChunk(new Vector3Int(5, 0, 4));

    }

	protected virtual void OnValidate()
	{
        noiseSettings.resolution = worldSettings.ChunkResolution + 1;
	}
}
