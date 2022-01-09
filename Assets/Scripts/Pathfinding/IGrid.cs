using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ValueChangedEvent<T>(int x, int y, T value);

public interface IGrid<T>
{
	public event ValueChangedEvent<T> OnValueChangedEvent;

	public int width { get; }
	public int height { get; }
	public int depth { get; }
	public int MaxSize { get; }

	public T this[int x, int y, int z] { get; set; }

	public T this[Vector3Int pos] { get; set; }

	public bool IsInBounds(int x, int y, int z);

	public bool IsInBounds(Vector3Int pos);
}
