using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
	protected static PathRequestManager instance;
	protected Pathfinding pathfinding;
	protected PhysicalGrid physicalGrid;
	protected bool isProcessingPath;

	protected void Awake()
	{
		instance = this;
		physicalGrid = GetComponent<PhysicalGrid>();
		pathfinding = physicalGrid.pathfinding;
	}

	public static Vector3[] GetPath(Vector3 start, Vector3 end)
	{
		List<PathNode> path = instance.pathfinding.FindPath(VoxelUtilities.ToVoxelPosition(start, instance.physicalGrid.world), VoxelUtilities.ToVoxelPosition(end, instance.physicalGrid.world));

		if (path == null) return new Vector3[0];

		Vector3[] simplifedPath = SimplifyPath(path);
		return simplifedPath;
	}

	protected static Vector3[] SimplifyPath(List<PathNode> path)
	{
		List<Vector3> waypoints = new List<Vector3>();
		Vector3Int directionOld = Vector3Int.zero;

		for (int i = 1; i < path.Count; i++)
		{
			Vector3Int directionNew = path[i - 1].gridPosition - path[i].gridPosition;

			if (directionNew != directionOld)
			{
				waypoints.Add(VoxelUtilities.ToWorldPosition(path[i - 1].gridPosition, instance.physicalGrid.world));
			}

			directionOld = directionNew;
		}
		waypoints.Add(VoxelUtilities.ToWorldPosition(path[path.Count - 1].gridPosition, instance.physicalGrid.world));

		return waypoints.ToArray();
	}
}
