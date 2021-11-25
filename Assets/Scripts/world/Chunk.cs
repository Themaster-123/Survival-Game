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

    public Voxel GetVoxel(Vector3Int position)
	{
        int index = GetVoxelIndex(position);
        return voxels[index];
	}

    public void SetVoxel(Vector3Int position, Voxel voxel)
	{
        int index = GetVoxelIndex(position);

        voxels[index] = voxel;
    }

    public void InitiateChunk(Vector3Int position, in World world)
	{
        this.position = position;
        this.world = world;

        NoiseSettings settings = world.noiseSettings;
        settings.offset += (Vector3)position * world.worldSettings.ChunkResolution;

        //float time = Time.realtimeSinceStartup;

        float[] voxelData = GpuNoise.GenerateNoise(settings);

        UpdateVoxels(voxelData);

        //print(Time.realtimeSinceStartup - time);


       // time = Time.realtimeSinceStartup;

        CreateMesh();

        //print(Time.realtimeSinceStartup - time);


        UpdateTransform();
    }

    public virtual void InitiateChunk(Vector3Int position, in Voxel[] voxels, in Vector3[] vertices, in int[] triangles, in World world)
    {
        this.position = position;
        this.world = world;
        this.voxels = voxels;

        UpdateMesh(vertices, triangles);
        
        UpdateTransform();
    }

    // regenerates the chunk's mesh
    public virtual void UpdateChunk()
	{
        MarchingCubes.GenerateMesh(Vector3Int.one * (world.worldSettings.ChunkResolution), .5f, voxels, out Vector3[] vertices, out int[] triangles, position, world);

        UpdateMesh(vertices, triangles);
    }

    // creates voxel array off of voxelData
    protected virtual void UpdateVoxels(in float[] voxelData)
	{
        voxels = GetVoxelsFromNoiseData(voxelData, world.worldSettings, Vector3Int.one * world.worldSettings.ChunkResolution);
    }

	protected virtual void Awake()
	{
        GetComponents();
	}

    protected virtual void LateUpdate()
	{
        UpdateChunk();
	}

    protected virtual int GetVoxelIndex(Vector3Int pos)
    {
        return (pos.z * world.worldSettings.ChunkResolution * world.worldSettings.ChunkResolution) + (pos.y * world.worldSettings.ChunkResolution) + pos.x;
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

        UpdateChunk();
	}

    protected virtual void UpdateMesh(in Vector3[] vertices, in int[] triangles)
	{
        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.triangles = triangles;
        meshFilter.mesh.RecalculateNormals();

        meshCollider.sharedMesh = meshFilter.mesh;
    }

    protected virtual void UpdateTransform()
    {
        transform.position = (Vector3)position * world.worldSettings.ChunkSize;
    }

    // gets chunk up forward and right neighbors
    public static Chunk[] GetConnectionChunkNeighbors(Vector3Int position, World world)
	{
        Vector3Int[] neighbors = { Vector3Int.forward, Vector3Int.up, Vector3Int.right};

        List<Chunk> chunkNeighbors = new List<Chunk>();

        for (int i = 0; i < neighbors.Length; i++)
		{
            Vector3Int pos = position + neighbors[i];

            Chunk chunk = world.GetChunk(pos);

            if (chunk != null)
			{
                chunkNeighbors.Add(chunk);
			}
		}

        return chunkNeighbors.ToArray();
	}

    public static Voxel[] GetVoxelsFromNoiseData(in float[] voxelData, in WorldSettings worldSettings, Vector3Int resolution)
	{
        Voxel[] voxels = new Voxel[voxelData.Length];

        for (int x = 0; x < resolution.x; x++)
        {
            for (int y = 0; y < resolution.y; y++)
            {
                for (int z = 0; z < resolution.z; z++)
                {
                    int index = (z * resolution.x * resolution.y) + (y * resolution.x) + x;
                    Voxel voxel;
                    voxel.value = voxelData[index];
                    voxels[index] = voxel;
                }
            }
        }

        return voxels;
    }
}
