using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Survival Game/World/Chunk")]
public class Chunk : MonoBehaviour
{
    readonly static float ChunkSize = 16;
    public Vector3Int position;
    public World world;

    public void InitiateChunk(Vector3Int position, in World world)
	{
        this.position = position;
        this.world = world;

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
