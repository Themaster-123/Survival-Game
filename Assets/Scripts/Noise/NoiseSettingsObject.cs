using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Noise Settings", menuName = "Survival Game/Noise Settings")]
public class NoiseSettingsObject : ScriptableObject
{
    public ComputeShader noiseShader;
    public uint octaves = 4;
    public float lacunarity = 2;
    public float persistence = .5f;

    protected void OnValidate()
    {
        octaves = System.Math.Max(octaves, 1);
    }
}
