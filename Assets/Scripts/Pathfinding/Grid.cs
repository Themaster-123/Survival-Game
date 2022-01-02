#define USE_OUT_OF_BOUNDS_CHECKS

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Grid
{
	public delegate void ValueChangedEvent(int x, int y, int value);

	public event ValueChangedEvent OnValueChangedEvent;

	public int width { get; protected set; }
	public int height { get; protected set; }

	protected int[,] gridArray;

	public bool IsInBounds(int x, int y)
	{
		if (x >= 0 && y >= 0 & x < width && y < height)
		{
			return true;
		}
		return false;
	}

	public bool IsInBounds(Vector2Int pos)
	{
		return IsInBounds(pos.x, pos.y);
	}

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
#if USE_OUT_OF_BOUNDS_CHECKS
			if (IsInBounds(x, y))
			{
#endif
				return gridArray[x, y];
#if USE_OUT_OF_BOUNDS_CHECKS
			}
			else
			{
				Debug.LogWarning("Attepting to get a value outside the range");
				return 0;
			}
#endif
		}

		set
		{
#if USE_OUT_OF_BOUNDS_CHECKS
			if (IsInBounds(x, y))
			{
#endif
			gridArray[x, y] = value;
			OnValueChangedEvent?.Invoke(x, y, value);
#if USE_OUT_OF_BOUNDS_CHECKS
			}
			else
			{
				Debug.LogWarning("Attepting to set a value outside the range");
			}
#endif
		}
	}

	public int this[Vector2Int pos]
	{
		get
		{
			return this[pos.x, pos.y];
		}

		set
		{
			this[pos.x, pos.y] = value;
		}
	}
}
