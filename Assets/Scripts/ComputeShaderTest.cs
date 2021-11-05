using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader shader;
    public Texture3D texture3d;
    [Header("Settings")]
    public int resolution;
    public Vector3 offset;
    public Vector3 size = new Vector3(1, 1, 1);
    public uint octaves;
    public float lacunarity;
    public float persistence;
    public float seed;

    protected float[] voxelData;
    ComputeBuffer voxelDataBuffer;


    public virtual void InitiateVoxels()
	{
        voxelData = new float[resolution * resolution * resolution];

        for (int i = 0; i < voxelData.Length; i++)
		{
            voxelData[i] = 0;
		}
	}

    [ContextMenu("Generate Noise")]
    public virtual void GenerateNoise()
	{
        if (texture3d == null)
        {
            texture3d = new Texture3D(resolution, resolution, resolution, TextureFormat.RGBA32, false);
            texture3d.filterMode = FilterMode.Point;
        }

        UpdateBuffer();

        DispatchShader();

        voxelDataBuffer.GetData(voxelData);

        SetVoxelDataTexture();
    }

    protected virtual void Start()
    {
        InitiateVoxels();
        GenerateNoise();
    }

	protected virtual void OnDisable()
	{
        voxelDataBuffer.Dispose();
	}

	protected virtual void Update()
    {

    }

	protected void OnValidate()
	{
        octaves = System.Math.Max(octaves, 1);
	}

	// creates the buffer if VoxelData length changed and set the buffer data to voxel data array
	protected virtual void UpdateBuffer()
	{
        if (voxelData == null)
        {
            InitiateVoxels();
        }

        if (voxelDataBuffer == null || voxelData.Length != voxelDataBuffer.count)
		{
            if (voxelDataBuffer != null)
			{
                voxelDataBuffer.Dispose();
            }

            voxelDataBuffer = new ComputeBuffer(voxelData.Length, sizeof(float));
        }
        voxelDataBuffer.SetData(voxelData);
    }

    // dispatches the compute shader
    protected virtual void DispatchShader()
	{
        shader.SetBuffer(0, "Result", voxelDataBuffer);
        shader.SetInt("Resolution", resolution);
        shader.SetVector("_Time", Shader.GetGlobalVector("_Time"));
        shader.SetVector("Offset", offset);
        shader.SetVector("Size", size);
        shader.SetInt("Octaves", (int)octaves);
        shader.SetFloat("Lacunarity", lacunarity);
        shader.SetFloat("Persistence", persistence);
        shader.SetFloat("Seed", seed);
        shader.Dispatch(0, resolution, resolution, resolution);
    }

    protected virtual void SetVoxelDataTexture()
	{
        Color32[] colors = new Color32[resolution * resolution * resolution];

        for (int i = 0; i < voxelData.Length; i++)
        {
            byte value = (byte)((Mathf.Clamp(voxelData[i], -1, 1) / 2.0f + .5f) * 255);
            colors[i] = new Color32(value, value, value, 255);
        }
        texture3d.SetPixels32(colors);
        texture3d.Apply();
    }
}
