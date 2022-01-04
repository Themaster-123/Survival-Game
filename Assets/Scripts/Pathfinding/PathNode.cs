using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
	public Vector2Int gridPosition;

	public int gCost;
	public int hCost;
	public int fCost;

	public PathNode prevNode;
	public bool walkable = true;

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

	public override string ToString()
	{
		return gridPosition.x + ", " + gridPosition.y;
	}
}
