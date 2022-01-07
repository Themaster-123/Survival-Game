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
		List<PathNode> path = instance.pathfinding.FindPath(instance.physicalGrid.GetGridPosition(start), instance.physicalGrid.GetGridPosition(end));
		Vector3[] simplifedPath = SimplifyPath(path);
		return simplifedPath;
	}

	protected static Vector3[] SimplifyPath(List<PathNode> path)
	{
		List<Vector3> waypoints = new List<Vector3>();
		Vector2Int directionOld = Vector2Int.zero;

		for (int i = 1; i < path.Count; i++)
		{
			Vector2Int directionNew = path[i - 1].gridPosition - path[i].gridPosition;

			if (directionNew != directionOld)
			{
				waypoints.Add(instance.physicalGrid.GetCenterWorldPosition(path[i - 1].gridPosition));
			}

			directionOld = directionNew;
		}
		waypoints.Add(instance.physicalGrid.GetCenterWorldPosition(path[path.Count - 1].gridPosition));

		return waypoints.ToArray();
	}
}
