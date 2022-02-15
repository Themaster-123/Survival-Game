using UnityEngine;
using Unity.Collections;
using System.Runtime.InteropServices;
using System;

[StructLayout(LayoutKind.Sequential)]
[Serializable]
public struct Voxel
{
	public float value;

	public Voxel(float value)
	{
		this.value = value;
	}
}
