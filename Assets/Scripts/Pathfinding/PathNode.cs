using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IHeapItem<PathNode>
{
	public Vector3Int gridPosition;

	public int gCost;
	public int hCost;
	public int fCost;

	public PathNode prevNode;
	public bool walkable = true;

	public int HeapIndex { get; set; }

	public PathNode(Vector3Int gridPosition)
	{
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
