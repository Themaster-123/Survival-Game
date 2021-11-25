using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelUtilities
{
	public static Vector3Int ToVoxelPosition(Vector3 pos, World world)
	{
		return Vector3Int.FloorToInt((pos / world.worldSettings.ChunkSize) * world.worldSettings.ChunkResolution);
	}
}
