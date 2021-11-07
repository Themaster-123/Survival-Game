using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Noise Settings", menuName = "Survival Game/Noise Settings")]
public class NoiseSettings : ScriptableObject
{
    public ComputeShader noiseShader;
    public int resolution = 32;
    public Vector3 offset;
    public Vector3 size = new Vector3(1, 1, 1);
    public uint octaves = 4;
    public float lacunarity = 2;
    public float persistence = .5f;
    public float seed;

    protected void OnValidate()
    {
        octaves = System.Math.Max(octaves, 1);
    }
}
