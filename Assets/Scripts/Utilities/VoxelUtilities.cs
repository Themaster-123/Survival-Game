using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelUtilities
{
	public static Vector3Int ToVoxelPosition(Vector3 pos, World world)
	{
		return Vector3Int.FloorToInt((pos / world.worldSettings.ChunkSize) * world.worldSettings.ChunkResolution);
	}

	public static float ClampVoxelValue(float value)
	{
		return Mathf.Clamp(value, -1, 1);
	}

	public static Vector3 ToWorldPosition(Vector3Int pos, World world)
	{
		return (pos + new Vector3(.5f, .5f, .5f)) * world.worldSettings.ChunkSize * world.worldSettings.InverseChunkResolution;
	}
	public static Vector3 ToWorldPosition(int x, int y, int z, World world)
	{
		return ToWorldPosition(new Vector3Int(x, y, z), world);
	}
}
