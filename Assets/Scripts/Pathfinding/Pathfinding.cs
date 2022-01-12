#define TIME_PATHFINDING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding
{
	public World world;

	protected const float VOXEL_VALUE_CUTOFF = 0;
	protected const int MOVE_STRAIGHT_COST = 10;
	protected const int MOVE_DIAGONAL_COST = 14;
	protected const int MOVE_3D_DIAGONAL_COST = 17;

	public Pathfinding(World world)
	{
		InitateValues(world);
	}

	public List<PathNode> FindPath(Vector3Int start, Vector3Int end)
	{
#if TIME_PATHFINDING
		Stopwatch sw = new Stopwatch();
		sw.Start();
#endif
		Dictionary<Vector3Int, PathNode> nodeSet = new Dictionary<Vector3Int, PathNode>();
		PathNode startNode = GetPathNodeFromVoxel(start, nodeSet);
		PathNode endNode = GetPathNodeFromVoxel(end, nodeSet);

		if (!startNode.walkable || !endNode.walkable) 
		{
#if TIME_PATHFINDING
			sw.Stop();
			MonoBehaviour.print("Path found: " + sw.ElapsedMilliseconds + " ms");
#endif
			return null; 
		}

		Heap<PathNode> openList = new Heap<PathNode>(world.worldSettings.ChunkResolution * world.worldSettings.ChunkResolution * world.worldSettings.ChunkResolution);
		HashSet<PathNode> closedList = new HashSet<PathNode>();
		openList.Add(startNode);

		startNode.gCost = 0;
		startNode.hCost = CalculateDistanceCost(startNode, endNode);

		while (openList.Count > 0)
		{
			PathNode currentNode = openList.RemoveFirst();
			if (currentNode == endNode)
			{
#if TIME_PATHFINDING
				sw.Stop();
				MonoBehaviour.print("Path found: " + sw.ElapsedMilliseconds + " ms");
#endif
				return GetFullPath(endNode);
			}

			closedList.Add(currentNode);

			foreach (PathNode neighbourNode in GetNeighbours(currentNode, nodeSet))
			{
				if (!neighbourNode.walkable || closedList.Contains(neighbourNode)) continue;

				Vector3Int localPosition = neighbourNode.gridPosition - currentNode.gridPosition;
				bool cornerCovered = true;

				for (int i = 0; i < 2; i++)
				{
					Vector3Int pos = currentNode.gridPosition;
					pos[i] += localPosition[i];
					if (GetPathNodeFromVoxel(pos, nodeSet).walkable)
					{
						cornerCovered = false;
					}
				}

				if (cornerCovered) continue;

				int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

				if (tentativeGCost < neighbourNode.gCost)
				{
					neighbourNode.prevNode = currentNode;
					neighbourNode.gCost = tentativeGCost;

					if (!openList.Contains(neighbourNode))
					{
						neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
						neighbourNode.CalculateFCost();
						openList.Add(neighbourNode);
					}
					else
					{
						neighbourNode.CalculateFCost();
						openList.UpdateItem(neighbourNode);
					}
				}
			}
		}

#if TIME_PATHFINDING
		sw.Stop();
		MonoBehaviour.print("Path found: " + sw.ElapsedMilliseconds + " ms");
#endif

		// Out of nodes!!!
		return null;
	}

	protected void InitateValues(World world)
	{
		this.world = world;
	}

	protected int CalculateDistanceCost(PathNode a, PathNode b)
	{
		Vector3Int distance = MathUtilities.Abs(a.gridPosition - b.gridPosition);
		int min = Mathf.Min(distance.x, distance.y, distance.z);
		int max = Mathf.Max(distance.x, distance.y, distance.z);
		int mid = distance.x + distance.y + distance.z - min - max;
		return MOVE_DIAGONAL_COST * (mid - min) + MOVE_STRAIGHT_COST * (max - mid) + MOVE_3D_DIAGONAL_COST * min;
	}

	protected PathNode GetLowestFCostNode(List<PathNode> openList)
	{
		PathNode lowestFCostNode = openList[0];
		for (int i = 1; i < openList.Count; i++)
		{
			if (openList[i].fCost < lowestFCostNode.fCost || (openList[i].fCost == lowestFCostNode.fCost && openList[i].hCost < lowestFCostNode.hCost)) lowestFCostNode = openList[i];
		}

		return lowestFCostNode;
	}

	protected List<PathNode> GetFullPath(PathNode node)
	{
		List<PathNode> path = new List<PathNode>() { node };
		PathNode currentNode = node;
		while (currentNode.prevNode != null)
		{
			path.Add(currentNode.prevNode);
			currentNode = currentNode.prevNode;
		}
		path.Reverse();
		return path;
	}

	protected List<PathNode> GetNeighbours(PathNode node, Dictionary<Vector3Int, PathNode> nodeSet)
	{
		List<PathNode> neighbourList = new List<PathNode>();
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				for (int z = -1; z <= 1; z++)
				{
					if (x == 0 && y == 0 && z == 0) continue;

					Vector3Int pos = node.gridPosition + new Vector3Int(x, y, z);

					neighbourList.Add(GetPathNodeFromVoxel(pos, nodeSet));
				}
			}
		}

		return neighbourList;
	}

	protected PathNode GetPathNodeFromVoxel(Vector3Int pos, Dictionary<Vector3Int, PathNode> nodeSet)
	{
		if (nodeSet.TryGetValue(pos, out PathNode pathNode)) return pathNode;

		pathNode = new PathNode(pos);

		pathNode.gCost = int.MaxValue;
		pathNode.CalculateFCost();
		pathNode.walkable = world.GetVoxel(pos).value > VOXEL_VALUE_CUTOFF ? false : true;

		nodeSet.Add(pos, pathNode);

		return pathNode;
	}
}
