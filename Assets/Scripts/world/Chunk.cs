using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Survival Game/World/Chunk")]
public class Chunk : MonoBehaviour
{
    readonly static float ChunkSize = 16;
    public Vector3Int position;

    public void InitiateChunk(Vector3Int position)
	{
        this.position = position;
        transform.position = (Vector3)position * ChunkSize;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = transform.position;
        cube.transform.localScale = new Vector3(ChunkSize, ChunkSize, ChunkSize);
    } 

    protected void Start()
    {
        
    }

    protected void Update()
    {
        
    }
}
