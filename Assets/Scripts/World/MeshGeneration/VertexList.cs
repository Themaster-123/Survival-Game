using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexList : IEnumerable<Vector3>
{
	private Vector3 vertex0;
	private Vector3 vertex1;
	private Vector3 vertex2;
	private Vector3 vertex3;
	private Vector3 vertex4;
	private Vector3 vertex5;
	private Vector3 vertex6;
	private Vector3 vertex7;
	private Vector3 vertex8;
	private Vector3 vertex9;
	private Vector3 vertex10;
	private Vector3 vertex11;

	public Vector3 this[int index]
	{
		get
		{
			return GetVertexByIndex(index);
		}
		set
		{
			GetVertexByIndex(index) = value;
		}
	}

	public IEnumerator<Vector3> GetEnumerator()
	{
		for (int i = 0; i < 12; i++)
		{
			yield return this[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	private ref Vector3 GetVertexByIndex(int index)
	{
		switch (index)
		{
			case 0: return ref vertex0;
			case 1: return ref vertex1;
			case 2: return ref vertex2;
			case 3: return ref vertex3;
			case 4: return ref vertex4;
			case 5: return ref vertex5;
			case 6: return ref vertex6;
			case 7: return ref vertex7;
			case 8: return ref vertex8;
			case 9: return ref vertex9;
			case 10: return ref vertex10;
			case 11: return ref vertex11;
			default: throw new ArgumentOutOfRangeException($"There are only 12 vertices! You tried to access the vertex at index {index}");
		}
	}
}
