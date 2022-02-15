using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Survival Game/World/Chunk")]
public class Chunk : MonoBehaviour
{
    [Header("Properties")]
    public Vector3Int position;
    public World world;

    [Header("Voxel Visualizer")]
    public bool ShowVoxels = true;
    public bool ShowNormals = true;
    public bool OnlyShowGroundVoxels = true;

    public MeshCollider meshCollider;
    protected MeshFilter meshFilter;
    protected Voxel[] voxels;
    protected bool voxelsChanged = false;

    public Voxel GetVoxel(Vector3Int position)
	{
        int index = GetVoxelIndex(position);
        return voxels[index];
	}

    public void SetVoxel(Vector3Int position, Voxel voxel)
	{
       // print(position);
        int index = GetVoxelIndex(position);
        voxel.value = VoxelUtils.ClampVoxelValue(voxel.value);
        voxels[index] = voxel;
        voxelsChanged = true;
    }

    public void InitiateChunk(Vector3Int position, in World world)
	{
        this.position = position;
        this.world = world;

        NoiseSettings settings = world.noiseSettings;
        settings.offset += (Vector3)position * world.worldSettings.ChunkResolution;

        float[] voxelData = GpuNoise.GenerateNoise(settings);

        UpdateVoxels(voxelData);

        CreateMesh();

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

    public virtual void ReloadModel()
	{
        voxelsChanged = true;
	}

    // regenerates the chunk's mesh
    public virtual void UpdateChunk()
	{
        //UpdateVoxelsPositions();

        MarchingCubes.GenerateMesh(Vector3Int.one * (world.worldSettings.ChunkResolution), 0, voxels, out Vector3[] vertices, out int[] triangles, position, world);

        UpdateMesh(vertices, triangles);
    }

    [ContextMenu("Test")]
    public virtual void Test()
	{
   

    }


    protected virtual Vector3Int GetWorldVoxelPosition(Vector3Int pos)
	{
        return pos + position * world.worldSettings.ChunkResolution;
    }

    protected virtual Vector3Int GetWorldVoxelPosition(int x, int y, int z)
    {
        return GetWorldVoxelPosition(new Vector3Int(x, y, z));
    }

    // creates voxel array off of voxelData
    protected virtual void UpdateVoxels(in float[] voxelData)
	{
        voxels = GetVoxelsFromNoiseData(voxelData, world.worldSettings, Vector3Int.one * world.worldSettings.ChunkResolution);
    }

    protected virtual void UpdateVoxelsPositions()
    {
        for (int x = 0; x < world.worldSettings.ChunkResolution; x++)
        {
            for (int y = 0; y < world.worldSettings.ChunkResolution; y++)
            {
                for (int z = 0; z < world.worldSettings.ChunkResolution; z++)
                {
                    int index = (z * world.worldSettings.ChunkResolution * world.worldSettings.ChunkResolution) + (y * world.worldSettings.ChunkResolution) + x;
                }
            }
        }
    }

    protected virtual void Awake()
	{
        GetComponents();
	}

    protected virtual void Update()
	{
	}

    protected void OnDrawGizmosSelected()
    {
        DrawVoxels();
        DrawNormals();
    }

	protected virtual void LateUpdate()
	{
        UpdateChunkIfChanged();
    }

    // updates the chunk if any voxels changed
    protected virtual void UpdateChunkIfChanged()
    {
        if (voxelsChanged)
        {
            voxelsChanged = false;
            UpdateChunk();
        }
    }

    protected virtual void CheckIfNeighbersChanged()
	{

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

    protected virtual void DrawVoxels()
	{
        for (int x = 0; x < world.worldSettings.ChunkResolution; x++)
        {
            for (int y = 0; y < world.worldSettings.ChunkResolution; y++)
            {
                for (int z = 0; z < world.worldSettings.ChunkResolution; z++)
                {
                    Voxel voxel = GetVoxel(new Vector3Int(x, y, z));
                    if (OnlyShowGroundVoxels && voxel.value <= 0) continue;
                    Gizmos.color = voxel.value <= 0 ? Color.white : Color.gray;
                    Gizmos.DrawWireCube(VoxelUtils.ToWorldPosition(GetWorldVoxelPosition(x, y, z), world), Vector3.one * world.worldSettings.ChunkSize * world.worldSettings.InverseChunkResolution);
                }
            }
        }
    }

    protected virtual void DrawNormals()
	{
        for (int x = 0; x < world.worldSettings.ChunkResolution; x++)
        {
            for (int y = 0; y < world.worldSettings.ChunkResolution; y++)
            {
                for (int z = 0; z < world.worldSettings.ChunkResolution; z++)
                {
                    Vector3Int pos = GetWorldVoxelPosition(x, y, z);

                    if (world.GetVoxel(pos).value <= 0) continue;

                    for (int nX = -1; nX <= 1; nX++)
                    {
                        for (int nY = -1; nY <= 1; nY++)
                        {
                            for (int nZ = -1; nZ <= 1; nZ++)
                            {
                                if (nX == 0 && nY == 0 && nZ == 0) continue;

                                Vector3Int offset = new Vector3Int(nX, nY, nZ);

                                int amountOfNonZeros = 0;
                                for (int i = 0; i < 3; i++)
                                {
                                    if (offset[i] != 0)
                                    {
                                        amountOfNonZeros++;
                                    }
                                }

                                if (amountOfNonZeros > 1) continue;

                                Vector3Int offsetPos = pos + offset;

                                if (world.GetVoxel(offsetPos).value <= 0f)
                                {
                                    goto LeaveNeighbourCheck;
                                }
                            }
                        }
                    }

                    continue;
                    LeaveNeighbourCheck:

                    Vector3 vector = MathUtils.FindGradientVector(new Vector3Int(x, y, z), (Vector3Int pos) => world.GetVoxel(position * world.worldSettings.ChunkResolution + pos).value);
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(VoxelUtils.ToWorldPosition(pos, world), -vector.normalized);

                }
            }
        }
    }

    // gets chunk up forward and right neighbors
    public static Chunk[] GetConnectionChunkNeighbors(Vector3Int position, World world)
	{
        Vector3Int[] neighbors = { Vector3Int.forward, Vector3Int.up, Vector3Int.right, Vector3Int.left, Vector3Int.down, Vector3Int.back};

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
                    voxel.value = VoxelUtils.ClampVoxelValue(-voxelData[index]);
                    voxels[index] = voxel;
                }
            }
        }

        return voxels;
    }
}
