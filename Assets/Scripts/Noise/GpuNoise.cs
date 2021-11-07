using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class GpuNoise
{

    public static float[] GenerateNoise(NoiseSettings settings)
	{
        float[] voxelData = InitiateVoxels(settings);
        ComputeBuffer buffer = CreateBuffer(voxelData);

        DispatchShader(settings, voxelData, buffer);

        buffer.GetData(voxelData);
        buffer.Dispose();
        return voxelData;
	}

	// creates the buffer if VoxelData length changed and set the buffer data to voxel data array
	protected static ComputeBuffer CreateBuffer(in float[] voxelData)
	{
        ComputeBuffer voxelDataBuffer = new ComputeBuffer(voxelData.Length, sizeof(float));
        voxelDataBuffer.SetData(voxelData);
        return voxelDataBuffer;
    }

    protected static float[] InitiateVoxels(NoiseSettings settings)
    {
        float[] voxelData = new float[settings.resolution * settings.resolution * settings.resolution];

        for (int i = 0; i < voxelData.Length; i++)
        {
            voxelData[i] = 0;
        }

        return voxelData;
    }

    // dispatches the compute shader
    protected static void DispatchShader(NoiseSettings settings, in float[] voxelData, in ComputeBuffer voxelDataBuffer)
	{
        settings.noiseShader.SetBuffer(0, "Result", voxelDataBuffer);
        settings.noiseShader.SetInt("Resolution", settings.resolution);
        settings.noiseShader.SetVector("_Time", Shader.GetGlobalVector("_Time"));
        settings.noiseShader.SetVector("Offset", settings.offset);
        settings.noiseShader.SetVector("Size", settings.size);
        settings.noiseShader.SetInt("Octaves", (int)settings.octaves);
        settings.noiseShader.SetFloat("Lacunarity", settings.lacunarity);
        settings.noiseShader.SetFloat("Persistence", settings.persistence);
        settings.noiseShader.SetFloat("Seed", settings.seed);
        settings.noiseShader.Dispatch(0, settings.resolution, settings.resolution, settings.resolution);
    }
}
