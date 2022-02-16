using UnityEngine;
using Unity.Collections;
using System.Runtime.InteropServices;
using System;

[StructLayout(LayoutKind.Sequential)]
[Serializable]
public struct Voxel
{
	public float value;
	public bool isBuilding;

	public Voxel(float value, bool isBuilding)
	{
		this.value = value;
		this.isBuilding = isBuilding;
	}

	public Voxel(float value) : this(value, false) { 
	}
}
