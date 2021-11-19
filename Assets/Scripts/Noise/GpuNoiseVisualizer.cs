using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpuNoiseVisualizer : MonoBehaviour
{
    public Texture3D texture3d;
    public NoiseSettings settings;
    public string nodeTree;

    [ContextMenu("Generate Noise")]
    public virtual void GenerateNoise()
    {
        if (texture3d == null)
        {
            texture3d = new Texture3D(settings.resolution, settings.resolution, settings.resolution, TextureFormat.RGBA32, false);
            texture3d.filterMode = FilterMode.Point;
        }

        FastNoise noise = FastNoise.FromEncodedNodeTree(nodeTree);



        //float[] voxelData = GpuNoise.GenerateNoise(settings);
        float[] voxelData = new float[settings.resolution * settings.resolution * settings.resolution];

        FastNoise.OutputMinMax minMax = noise.GenUniformGrid3D(voxelData, (int)settings.offset.x, (int)settings.offset.y, (int)settings.offset.z, settings.resolution, settings.resolution, settings.resolution, settings.size.x, (int)settings.seed);


        SetVoxelDataTexture(voxelData);
    }

    // updates the 3d texture
    protected virtual void SetVoxelDataTexture(in float[] voxelData)
    {
        Color32[] colors = new Color32[settings.resolution * settings.resolution * settings.resolution];

        for (int i = 0; i < voxelData.Length; i++)
        {
            byte value = (byte)((Mathf.Clamp(voxelData[i], -1, 1) / 2 + .5f) * 255);
            colors[i] = new Color32(value, value, value, 255);
        }
        texture3d.SetPixels32(colors);
        texture3d.Apply();
    }

}
