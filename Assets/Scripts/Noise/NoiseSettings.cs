using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct NoiseSettings
{
    public NoiseSettingsObject settingsObject;
    public int resolution;
    public Vector3 offset;
    public Vector3 size;
    public float seed;

    public NoiseSettings(NoiseSettingsObject settingsObject, int resolution, Vector3 offset, Vector3 size, float seed)
	{
        this.settingsObject = settingsObject;
        this.resolution = resolution;
        this.offset = offset;
        this.size = size;
        this.seed = seed;
	}
}
