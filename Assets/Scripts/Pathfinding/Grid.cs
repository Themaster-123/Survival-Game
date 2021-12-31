using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Grid
{
	public int width { get; protected set; }
	public int height { get; protected set; }

	protected int[,] gridArray;

	public Grid(int width, int height)
	{
		this.width = width;
		this.height = height;

		gridArray = new int[width, height];
	}

	public Grid(int[,] gridArray)
	{
		this.gridArray = gridArray.Clone() as int[,];

		this.width = gridArray.GetLength(0);
		this.height = gridArray.GetLength(1);
	}

	public int this[int x, int y]
	{
		get
		{
			return gridArray[x, y];
		}

		set
		{
			gridArray[x, y] = value;
		}
	}
}
