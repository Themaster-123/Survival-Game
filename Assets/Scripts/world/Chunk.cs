using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Survival Game/World/Chunk")]
public class Chunk : MonoBehaviour
{
    public Vector3Int position;
    public World world;

    protected MeshCollider meshCollider;
    protected MeshFilter meshFilter;
    protected Voxel[] voxels;

    public void InitiateChunk(Vector3Int position, in World world)
	{
        this.position = position;
        this.world = world;

        NoiseSettings settings = world.noiseSettings;
        settings.offset += (Vector3)position * world.worldSettings.ChunkResolution;

        float[] voxelData = GpuNoise.GenerateNoise(settings);

        UpdateVoxels(voxelData);

        CreateMesh();

        transform.position = (Vector3)position * world.worldSettings.ChunkSize;
    } 

    // creates voxel array off of voxelData
    protected virtual void UpdateVoxels(in float[] voxelData)
	{
        voxels = new Voxel[voxelData.Length];

        // i do this to fix the seam problem
        int increasedResolution = world.worldSettings.ChunkResolution + 1;

        for (int x = 0; x < increasedResolution; x++)
		{
            for (int y = 0; y < increasedResolution; y++)
            {
                for (int z = 0; z < increasedResolution; z++)
                {
                    int index = x + increasedResolution * (y + increasedResolution * z);
                    Voxel voxel;
                    voxel.position = new Vector3(x, y, z) / world.worldSettings.ChunkResolution * world.worldSettings.ChunkSize;
                    voxel.value = voxelData[index];
                    voxels[index] = voxel;
                }
            }
        }
    }

	protected virtual void Awake()
	{
        GetComponents();
	}

    protected virtual void GetComponents()
	{
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
	}

	// creates mesh off of voxel data
	protected virtual void CreateMesh()
	{
        Mesh chunkMesh = new Mesh();
        meshFilter.mesh = chunkMesh;
        meshCollider.sharedMesh = chunkMesh;

		MarchingCubes.GenerateMesh(Vector3Int.one * (world.worldSettings.ChunkResolution + 1), .5f, voxels, out Vector3[] vertices, out int[] triangles);

		UpdateMesh(vertices, triangles);
	}

    protected virtual void UpdateMesh(in Vector3[] vertices, in int[] triangles)
	{
        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.triangles = triangles;
        meshFilter.mesh.RecalculateNormals();
    }
}
