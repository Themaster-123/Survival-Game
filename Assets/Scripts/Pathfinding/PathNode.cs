using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
	public int gCost;
	public int hCost;
	public int fCost;
	public PathNode prevNode;

	protected Grid<PathNode> grid;
	protected Vector2Int gridPosition;

	public PathNode(in Grid<PathNode> grid, Vector2Int gridPosition)
	{
		this.grid = grid;
		this.gridPosition = gridPosition;
	}

	public override string ToString()
	{
		return gridPosition.x + ", " + gridPosition.y;
	}
}
