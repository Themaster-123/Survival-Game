#define USE_OUT_OF_BOUNDS_CHECKS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Grid<T> : IGrid<T>
{
	public event ValueChangedEvent<T> OnValueChangedEvent;

	public int width { get; }
	public int height { get; }

	protected T[,] gridArray;

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

		gridArray = new T[width, height];
	}

	public Grid(int width, int height, Func<Grid<T>, int, int, T> instaniateGridObject)
	{
		this.width = width;
		this.height = height;

		gridArray = new T[width, height];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				gridArray[x, y] = instaniateGridObject(this, x, y);
			}
		}
	}

	public Grid(T[,] gridArray)
	{
		this.gridArray = gridArray.Clone() as T[,];

		this.width = gridArray.GetLength(0);
		this.height = gridArray.GetLength(1);
	}

	public T this[int x, int y]
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
				return default(T);
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

	public T this[Vector2Int pos]
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
