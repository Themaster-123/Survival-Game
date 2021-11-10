using UnityEngine;
using Unity.Collections;

public struct Voxel
{
	public Vector3 position;
	public float value;

	public Voxel(Vector3 position, float value)
	{
		this.position = position;
		this.value = value;
	}
}
