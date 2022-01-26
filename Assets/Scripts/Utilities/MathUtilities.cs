using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtilities
{
	public static float limit = 0.000001f;

	public static float Mod(float a, float b)
	{
		float r = a % b;
		return r < 0 ? r + b : r;
	}

	public static int Mod(int a, int b)
	{
		int r = a % b;
		return r < 0 ? r + b : r;
	}

	public static Vector3 Abs(Vector3 vector)
	{
		return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	}

	public static Vector2 Abs(Vector2 vector)
	{
		return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
	}

	public static Vector3Int Abs(Vector3Int vector)
	{
		return new Vector3Int(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	}

	public static Vector2Int Abs(Vector2Int vector)
	{
		return new Vector2Int(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
	}

	public static float FindDerivative(int axis, Func<int, float> function)
	{
		return (function(axis + 1) - function(axis - 1)) / 2;
	}

	public static Vector3 FindGradientVector(Vector3Int pos, Func<Vector3Int, float> function)
	{
		float divX = FindDerivative(pos.x, (int x) => function(new Vector3Int(x, pos.y, pos.z)));
		float divY = FindDerivative(pos.y, (int y) => function(new Vector3Int(pos.x, y, pos.z)));
		float divZ = FindDerivative(pos.z, (int z) => function(new Vector3Int(pos.x, pos.y, z)));

		return new Vector3(divX, divY, divZ);
	}

	// creates a new Vector2 with the old vector's x and z
	public static Vector2 Flatten(Vector3 vector)
	{
		return new Vector2(vector.x, vector.z);
	}

	public static Vector3 UnFlatten(Vector2 vector)
	{
		return new Vector3(vector.x, 0, vector.y);
	}
}
