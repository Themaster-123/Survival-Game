using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Survival Game/World/World")]
public class World : MonoBehaviour
{
    protected Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    public void LoadChunk(Vector3Int chunkPosition)
	{
        if (!IsChunkLoaded(chunkPosition))
		{
            GameObject chunkObject = new GameObject(chunkPosition.ToString());
            chunkObject.transform.parent = transform;
            Chunk chunk = chunkObject.AddComponent<Chunk>();
            chunk.InitiateChunk(chunkPosition);
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

    }

    // Update is called once per frame
    protected void Update()
    {
        
    }
}
