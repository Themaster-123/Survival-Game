using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

public struct ChunkData
{
    public Vector3Int position;
    public Vector3[] vertices;
    public int[] triangles;
    public Voxel[] voxels;

    public ChunkData(Vector3Int position, Vector3[] vertices, int[] triangles, Voxel[] voxels)
	{
        this.position = position;
        this.vertices = vertices;
        this.triangles = triangles;
        this.voxels = voxels;
	}
}
