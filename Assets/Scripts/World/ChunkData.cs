using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

public struct ChunkData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Voxel[] voxels;
    public Vector3Int position;

    public ChunkData(in Vector3[] vertices, in int[] triangles, in Voxel[] voxels, Vector3Int position)
	{
        this.vertices = vertices;
        this.triangles = triangles;
        this.voxels = voxels;
        this.position = position;
	}
}
