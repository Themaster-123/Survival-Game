using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
	public readonly Vector3[] waypoints;
	public readonly Line[] turnBoundaries;

	public Path(Vector3[] pWaypoints, Vector3 startPos, float turnDistance)
	{
		waypoints = pWaypoints;
		turnBoundaries = new Line[waypoints.Length];

		Vector2 prevPoint = MathUtilities.Flatten(startPos);
		for (int i = 0; i < waypoints.Length; i++)
		{
			Vector2 currPoint = MathUtilities.Flatten(waypoints[i]);
			// direction to current point
			Vector2 dir = (currPoint - prevPoint).normalized;
			Vector2 turnBoundPoint = i == waypoints.Length - 1 ? currPoint : currPoint - dir * turnDistance;
			turnBoundaries[i] = new Line(turnBoundPoint, prevPoint - dir * turnDistance);
			prevPoint = turnBoundPoint;
		}
	}

	public void DrawGizmos()
	{
		Gizmos.color = Color.black;
		foreach (Vector3 point in waypoints)
		{
			Gizmos.DrawCube(point + Vector3.up, Vector3.one);
		}

		Gizmos.color = Color.white;
		foreach (Line line in turnBoundaries)
		{
			line.DrawGizmos(10);
		}
	}
}
