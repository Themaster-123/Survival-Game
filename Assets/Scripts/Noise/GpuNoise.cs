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
        settings.settingsObject.noiseShader.SetBuffer(0, "Result", voxelDataBuffer);
        settings.settingsObject.noiseShader.SetInt("Resolution", settings.resolution);
        settings.settingsObject.noiseShader.SetVector("_Time", Shader.GetGlobalVector("_Time"));
        settings.settingsObject.noiseShader.SetVector("Offset", settings.offset);
        settings.settingsObject.noiseShader.SetVector("Size", settings.size);
        settings.settingsObject.noiseShader.SetInt("Octaves", (int)settings.settingsObject.octaves);
        settings.settingsObject.noiseShader.SetFloat("Lacunarity", settings.settingsObject.lacunarity);
        settings.settingsObject.noiseShader.SetFloat("Persistence", settings.settingsObject.persistence);
        settings.settingsObject.noiseShader.SetFloat("Seed", settings.seed);
        settings.settingsObject.noiseShader.Dispatch(0, settings.resolution, settings.resolution, settings.resolution);
    }
}
