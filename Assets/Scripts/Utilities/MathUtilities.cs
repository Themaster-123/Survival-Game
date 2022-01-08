using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtilities
{
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
}
