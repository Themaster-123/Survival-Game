using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public Grid<PathNode> grid;

	protected const int MOVE_STRAIGHT_COST = 10  ;
	protected const int MOVE_DIAGONAL_COST = 14;

	public Pathfinding(int width, int height)
	{
		InitateGrid(width, height);
	}

	public List<PathNode> FindPath(Vector2Int start, Vector2Int end)
	{
		PathNode startNode = grid[start];
		PathNode endNode = grid[end];

		List<PathNode> openList = new List<PathNode>() { startNode };
		HashSet<PathNode> closedList = new HashSet<PathNode>();

		for (int x = 0; x < grid.width; x++)
		{
			for (int y = 0; y < grid.height; y++)
			{
				PathNode pathNode = grid[x, y];
				pathNode.gCost = int.MaxValue;
				pathNode.CalculateFCost();
				pathNode.prevNode = null;
			}
		}

		startNode.gCost = 0;
		startNode.hCost = CalculateDistanceCost(startNode, endNode);

		while (openList.Count > 0)
		{
			PathNode currentNode = GetLowestFCostNode(openList);
			if (currentNode == endNode)
			{
				return GetFullPath(endNode);
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);

			foreach (PathNode neighbourNode in GetNeighbours(currentNode))
			{
				if (!neighbourNode.walkable || closedList.Contains(neighbourNode)) continue;

				Vector2Int localPosition = neighbourNode.gridPosition - currentNode.gridPosition;
				bool cornerCovered = true;

				for (int i = 0; i < 2; i++)
				{
					Vector2Int pos = currentNode.gridPosition;
					pos[i] += localPosition[i];
					if (grid[pos].walkable)
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
					neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
					neighbourNode.CalculateFCost();

					if (!openList.Contains(neighbourNode))
					{
						openList.Add(neighbourNode);
					}
				}
			}
		}

		// Out of nodes!!!
		return null;
	}

	protected void InitateGrid(int width, int height)
	{
		grid = new Grid<PathNode>(width, height, (Grid<PathNode> grid, int x, int y) => new PathNode(grid, new Vector2Int(x, y)));

	}

	protected int CalculateDistanceCost(PathNode a, PathNode b)
	{
		int xDistance = Mathf.Abs(a.gridPosition.x - b.gridPosition.x);
		int yDistance = Mathf.Abs(a.gridPosition.y - b.gridPosition.y);
		int remaining = Mathf.Abs(xDistance - yDistance);
		return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
	}

	protected PathNode GetLowestFCostNode(List<PathNode> openList)
	{
		PathNode lowestFCostNode = openList[0];
		for (int i = 1; i < openList.Count; i++)
		{
			if (openList[i].fCost < lowestFCostNode.fCost) lowestFCostNode = openList[i];
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

	protected List<PathNode> GetNeighbours(PathNode node)
	{
		List<PathNode> neighbourList = new List<PathNode>();
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0) continue;

				Vector2Int pos = node.gridPosition + new Vector2Int(x, y);

				if (grid.IsInBounds(pos))
				{
					neighbourList.Add(grid[pos]);
				}
			}
		}

		return neighbourList;
	}
}
