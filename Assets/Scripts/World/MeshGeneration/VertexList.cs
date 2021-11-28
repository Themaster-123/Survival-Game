using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VertexList : IEnumerable<Vector3>
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
			switch (index)
			{
				case 0: return vertex0;
				case 1: return vertex1;
				case 2: return vertex2;
				case 3: return vertex3;
				case 4: return vertex4;
				case 5: return vertex5;
				case 6: return vertex6;
				case 7: return vertex7;
				case 8: return vertex8;
				case 9: return vertex9;
				case 10: return vertex10;
				case 11: return vertex11;
				default: throw new ArgumentOutOfRangeException($"There are only 12 vertices! You tried to access the vertex at index {index}");
			}
		}
		set
		{
			switch (index)
			{
				case 0: vertex0 = value;
					break;
				case 1: vertex1 = value;
					break;
				case 2: vertex2 = value;
					break;
				case 3: vertex3 = value;
					break;
				case 4: vertex4 = value;
					break;
				case 5: vertex5 = value;
					break;
				case 6: vertex6 = value;
					break;
				case 7: vertex7 = value;
					break;
				case 8: vertex8 = value;
					break;
				case 9: vertex9 = value;
					break;
				case 10: vertex10 = value;
					break;
				case 11: vertex11 = value;
					break;
				default: throw new ArgumentOutOfRangeException($"There are only 12 vertices! You tried to access the vertex at index {index}");
			}
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
}
