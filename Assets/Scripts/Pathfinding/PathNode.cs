using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IHeapItem<PathNode>
{
	public Vector2Int gridPosition;

	public int gCost;
	public int hCost;
	public int fCost;

	public PathNode prevNode;
	public bool walkable = true;

	public int HeapIndex { get; set; }

	protected Grid<PathNode> grid;

	public PathNode(in Grid<PathNode> grid, Vector2Int gridPosition)
	{
		this.grid = grid;
		this.gridPosition = gridPosition;
	}

	public void CalculateFCost()
	{
		fCost = gCost + hCost;
	}

	public int CompareTo(PathNode other)
	{
		int compare = fCost.CompareTo(other.fCost);
		if (compare == 0)
		{
			compare = hCost.CompareTo(other.hCost);
		}

		return -compare;
	}

	public override string ToString()
	{
		return gridPosition.x + ", " + gridPosition.y;
	}
}
