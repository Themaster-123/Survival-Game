using UnityEngine;

public static class ChunkPositionUtils
{
	public static uint Distance(Vector3Int pos1, Vector3Int pos2)
	{
		Vector3Int localPos = pos1 - pos2;
		int distance = 0;
		
		for (int i = 0; i < 3; i++)
		{
			int tempDistance = Mathf.Abs(localPos[i]);
			if (distance < tempDistance)
			{
				distance = tempDistance;
			}
		}

		return (uint)distance;
	}

	public static Vector3Int ToChunkPosition(Vector3 pos, World world)
	{
        Vector3Int chunkPosition = Vector3Int.zero;

        if (pos.x > 0)
        {
            chunkPosition.x = (int)(pos.x / world.worldSettings.ChunkSize);
        }
        else
        {
            chunkPosition.x = (int)Mathf.Floor(pos.x / world.worldSettings.ChunkSize);
        }

        if (pos.y > 0)
        {
            chunkPosition.y = (int)(pos.y / world.worldSettings.ChunkSize);
        }
        else
        {
            chunkPosition.y = (int)Mathf.Floor(pos.y / world.worldSettings.ChunkSize);
        }

        if (pos.z > 0)
        {
            chunkPosition.z = (int)(pos.z / world.worldSettings.ChunkSize);
        }
        else
        {
            chunkPosition.z = (int)Mathf.Floor(pos.z / world.worldSettings.ChunkSize);
        }

        return chunkPosition;
    }

    public static Vector3Int VoxelToChunkPosition(Vector3Int pos, World world)
	{
        return Vector3Int.FloorToInt((Vector3)pos * world.worldSettings.InverseChunkResolution);
	}
}
