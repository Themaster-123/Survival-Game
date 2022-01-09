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
	public int depth { get; }

	public int MaxSize { get { return width * height * depth; } }

	protected T[,,] gridArray;

	public bool IsInBounds(int x, int y, int z)
	{
		if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth)
		{
			return true;
		}
		return false;
	}

	public bool IsInBounds(Vector3Int pos)
	{
		return IsInBounds(pos.x, pos.y, pos.z);
	}

	public Grid(int width, int height, int depth)
	{
		this.width = width;
		this.height = height;
		this.depth = depth;

		gridArray = new T[width, height, depth];
	}

	public Grid(int width, int height, int depth, Func<Grid<T>, int, int, int, T> instaniateGridObject)
	{
		this.width = width;
		this.height = height;
		this.depth = depth;

		gridArray = new T[width, height, depth];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < depth; z++)
				{
					gridArray[x, y, z] = instaniateGridObject(this, x, y, z);

				}
			}
		}
	}

	public Grid(T[,,] gridArray)
	{
		this.gridArray = gridArray.Clone() as T[,,];

		this.width = gridArray.GetLength(0);
		this.height = gridArray.GetLength(1);
	}

	public T this[int x, int y, int z]
	{
		get
		{
#if USE_OUT_OF_BOUNDS_CHECKS
			if (IsInBounds(x, y, z))
			{
#endif
				return gridArray[x, y, z];
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
			if (IsInBounds(x, y, z))
			{
#endif
			gridArray[x, y, z] = value;
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

	public T this[Vector3Int pos]
	{
		get
		{
			return this[pos.x, pos.y, pos.z];
		}

		set
		{
			this[pos.x, pos.y, pos.z] = value;
		}
	}
}
