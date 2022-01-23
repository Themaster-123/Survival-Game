#define TIME_PATHFINDING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;


public class Pathfinding
{
	public delegate void endNodeAction(ref PathNode node, Voxel voxel, Dictionary<Vector3Int, PathNode> nodeSet);

	public World world;

	protected readonly static float VOXEL_VALUE_CUTOFF = 0;
	protected readonly static int MOVE_STRAIGHT_COST = 10;
	protected readonly static int MOVE_DIAGONAL_COST = 14;
	protected readonly static int MOVE_3D_DIAGONAL_COST = 17;

	public Pathfinding(World world)
	{
		InitateValues(world);
	}

	public List<PathNode> FindPath(Vector3Int start, Vector3Int end, Func<Vector3Int, Dictionary<Vector3Int, PathNode>, PathNode> pathNodeFromVoxel,
		Func<PathNode, PathNode, bool> walkableException, endNodeAction tryMakeEndNodeWalkable)
	{
#if TIME_PATHFINDING
		Stopwatch sw = new Stopwatch();
		sw.Start();
#endif
		Dictionary<Vector3Int, PathNode> nodeSet = new Dictionary<Vector3Int, PathNode>();
		PathNode startNode = pathNodeFromVoxel(start, nodeSet);
		PathNode endNode = pathNodeFromVoxel(end, nodeSet);

		if (!endNode.walkable) 
		{
			Voxel endVoxel = world.GetVoxel(end);
			nodeSet.Remove(endNode.gridPosition);
			tryMakeEndNodeWalkable(ref endNode, endVoxel, nodeSet);
			if (!endNode.walkable)
			{
#if TIME_PATHFINDING
				sw.Stop();
				MonoBehaviour.print("Path found: " + sw.ElapsedMilliseconds + " ms");
#endif
				return null;
			}
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

			foreach (PathNode neighbourNode in GetNeighbours(currentNode, nodeSet, pathNodeFromVoxel))
			{
				if ((!walkableException(currentNode, neighbourNode) && !neighbourNode.walkable) || closedList.Contains(neighbourNode)) continue;

				Vector3Int localPosition = neighbourNode.gridPosition - currentNode.gridPosition;

				// might add back?
/*				bool cornerCovered = true;

				for (int i = 0; i < 3; i++)
				{
					Vector3Int pos = currentNode.gridPosition;
					pos[i] += localPosition[i];
					if (pathNodeFromVoxel(pos, nodeSet).walkable)
					{
						cornerCovered = false;
					}
				}

				if (cornerCovered) continue;*/

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

	public List<PathNode> FindPath(Vector3Int start, Vector3Int end)
	{
		return FindPath(start, end, GetPathNodeFromVoxel, (_a, _b) => false, delegate(ref PathNode _a, Voxel _b, Dictionary<Vector3Int, PathNode> nodeSet) { });
	}

	public List<PathNode> FindPathOnGround(Vector3Int start, Vector3Int end, float maxSlope)
	{
		return FindPath(start, end, (Vector3Int pos, Dictionary<Vector3Int, PathNode> hashSet) =>
		{
			return GetPathNodeFromVoxel(pos, hashSet, (Voxel voxel, Vector3Int pos) => IsVoxelOnGround(voxel, pos, maxSlope));
		}, (currentNode, neighbourNode) =>
		{
			if (currentNode.gridPosition.x == neighbourNode.gridPosition.x && currentNode.gridPosition.z == neighbourNode.gridPosition.z || world.GetVoxel(currentNode.gridPosition + Vector3Int.down).value > 0)
			{
				if (neighbourNode.gridPosition.y < currentNode.gridPosition.y && world.GetVoxel(neighbourNode.gridPosition).value <= 0)
				{
					return true;
				}
			}

			return false;
		}, delegate (ref PathNode node, Voxel voxel, Dictionary<Vector3Int, PathNode> nodeSet)
		{
			if (voxel.value > 0)
			{
				Vector3Int topVoxelPosition = node.gridPosition + Vector3Int.up;
				while (world.GetVoxel(topVoxelPosition).value > 0)
				{
					topVoxelPosition += Vector3Int.up;
				}

				node = GetPathNodeFromVoxel(topVoxelPosition + Vector3Int.down, nodeSet, (Voxel voxel, Vector3Int pos) => IsVoxelOnGround(voxel, pos, maxSlope));
			} else
			{
				Vector3Int bottomVoxelPosition = node.gridPosition + Vector3Int.down;
				while (world.GetVoxel(bottomVoxelPosition).value <= 0)
				{
					bottomVoxelPosition += Vector3Int.down;
				}

				node = GetPathNodeFromVoxel(bottomVoxelPosition, nodeSet, (Voxel voxel, Vector3Int pos) => IsVoxelOnGround(voxel, pos, maxSlope));
			}
		});
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

	protected List<PathNode> GetNeighbours(PathNode node, Dictionary<Vector3Int, PathNode> nodeSet, Func<Vector3Int, Dictionary<Vector3Int, PathNode>, PathNode> pathNodeFromVoxel)
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

					neighbourList.Add(pathNodeFromVoxel(pos, nodeSet));
				}
			}
		}

		return neighbourList;
	}

	protected PathNode GetPathNodeFromVoxel(Vector3Int pos, Dictionary<Vector3Int, PathNode> nodeSet, Func<Voxel, Vector3Int, bool> isWalkable)
	{
		PathNode pathNode;

		if (nodeSet.TryGetValue(pos, out pathNode)) return pathNode;

		pathNode = new PathNode(pos);

		pathNode.gCost = int.MaxValue;
		pathNode.CalculateFCost();
		pathNode.walkable = isWalkable(world.GetVoxel(pos), pos);

		nodeSet.Add(pos, pathNode);

		return pathNode;
	}

	protected PathNode GetPathNodeFromVoxel(Vector3Int pos, Dictionary<Vector3Int, PathNode> nodeSet)
	{
		return GetPathNodeFromVoxel(pos, nodeSet, (Voxel voxel, Vector3Int pos) => voxel.value > VOXEL_VALUE_CUTOFF ? false : true);
	}

	protected bool IsVoxelOnGround(Voxel voxel, Vector3Int pos, float maxSlope)
	{
		if (voxel.value <= 0) return false;

		for (int nX = -1; nX <= 1; nX++)
		{
			for (int nY = -1; nY <= 1; nY++)
			{
				for (int nZ = -1; nZ <= 1; nZ++)
				{
					if (nX == 0 && nY == 0 && nZ == 0) continue;

					Vector3Int offset = new Vector3Int(nX, nY, nZ);

					int amountOfNonZeros = 0;
					for (int i = 0; i < 3; i++)
					{
						if (offset[i] != 0)
						{
							amountOfNonZeros++;
						}
					}

					if (amountOfNonZeros > 1) continue;

					Vector3Int offsetPos = pos + offset;

					if (world.GetVoxel(offsetPos).value <= 0f)
					{
						Vector3 normal = -MathUtilities.FindGradientVector(pos, (Vector3Int pos) => world.GetVoxel(pos).value);
						normal.Normalize();
						if (normal != Vector3.zero && Vector3.Angle(Vector3.up, normal) <= maxSlope) return true;
						return false;
					}
				}
			}
		}

		return false;
	}
}
