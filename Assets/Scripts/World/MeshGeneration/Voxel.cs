using UnityEngine;
using Unity.Collections;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct Voxel
{
	public Vector3 position;
	public float value;

	public Voxel(Vector3 position, float value)
	{
		this.position = position;
		this.value = value;
	}

	public Voxel(float value)
	{
		this.position = Vector3.zero;
		this.value = value;
	}
}
