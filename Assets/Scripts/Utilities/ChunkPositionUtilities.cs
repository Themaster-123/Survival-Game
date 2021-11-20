using UnityEngine;

public static class ChunkPositionUtilities
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
}
