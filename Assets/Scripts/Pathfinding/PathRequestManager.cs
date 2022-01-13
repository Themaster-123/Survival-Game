using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
	protected static PathRequestManager instance;
	protected Thread pathRequestThread;
	protected Pathfinding pathfinding;
	protected PhysicalGrid physicalGrid;
	protected bool isProcessingPath;
	protected ConcurrentQueue<PathRequest> pathRequestQueue = new ConcurrentQueue<PathRequest>();
	protected ConcurrentQueue<PathResult> pathResultsQueue = new ConcurrentQueue<PathResult>();

	protected void Awake()
	{
		InitiateFields();
	}

	protected void OnEnable()
	{
		StartThread();
	}

	protected void OnDisable()
	{
		
	}

	protected void FixedUpdate()
	{
		while (pathResultsQueue.TryDequeue(out PathResult pathResult))
		{
			pathResult.callback?.Invoke(pathResult.path);
		}
	}

	public static void GetPath(Vector3 start, Vector3 end, Action<Vector3[]> callback)
	{
		instance.pathRequestQueue.Enqueue(new PathRequest(start, end, callback));
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
				waypoints.Add(VoxelUtilities.ToWorldPosition(path[i - 1].gridPosition, instance.physicalGrid.World));
			}

			directionOld = directionNew;
		}
		waypoints.Add(VoxelUtilities.ToWorldPosition(path[path.Count - 1].gridPosition, instance.physicalGrid.World));

		return waypoints.ToArray();
	}


	protected void InitiateFields()
	{
		instance = this;
		physicalGrid = GetComponent<PhysicalGrid>();
		pathfinding = physicalGrid.pathfinding;
	}

	protected void StartThread()
	{
		pathRequestThread = new Thread(PathfindingLoop);
		pathRequestThread.Name = "Pathfinding Thread";
		pathRequestThread.Start();
	}

	protected void EndThread()
	{
		pathRequestThread.Abort();
	}

	protected void PathfindingLoop()
	{
		while (true)
		{
			while (pathRequestQueue.TryDequeue(out PathRequest pathRequest))
			{
				List<PathNode> path = instance.pathfinding.FindPath(VoxelUtilities.ToVoxelPosition(pathRequest.start, instance.physicalGrid.World), VoxelUtilities.ToVoxelPosition(pathRequest.end, instance.physicalGrid.World));

				Vector3[] simplifedPath;

				if (path == null) simplifedPath = new Vector3[0];
				else simplifedPath = SimplifyPath(path);

				pathResultsQueue.Enqueue(new PathResult(simplifedPath, pathRequest.callback));
			}
		}
	}

	protected struct PathResult
	{
		public Vector3[] path;
		public Action<Vector3[]> callback;

		public PathResult(Vector3[] path, Action<Vector3[]> callback)
		{
			this.path = path;
			this.callback = callback;
		}
	}

	protected struct PathRequest
	{
		public Vector3 start;
		public Vector3 end;
		public Action<Vector3[]> callback;

		public PathRequest(Vector3 start, Vector3 end, Action<Vector3[]> callback)
		{
			this.start = start;
			this.end = end;
			this.callback = callback;
		}
	}
}

