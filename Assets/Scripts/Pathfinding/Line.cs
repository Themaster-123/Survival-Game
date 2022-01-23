using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
	public readonly static float VERTICAL_LINE_GRADIENT = 1e10f;

	private float gradient;
	private float yIntercept;
	private float gradientPerpendicular;

	private Vector2 pointOnLine1;
	private Vector2 pointOnLine2;
	private bool approachSide;

	public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
	{
		Vector2 dPoint = pointOnLine - pointPerpendicularToLine;
		
		if (dPoint.x == 0)
		{
			gradientPerpendicular = VERTICAL_LINE_GRADIENT;
		}
		else
		{
			gradientPerpendicular = dPoint.y / dPoint.x;
		}
		
		if (gradientPerpendicular == 0)
		{
			gradient = VERTICAL_LINE_GRADIENT;
		}
		else
		{
			gradient = -1 / gradientPerpendicular;
		}

		yIntercept = pointOnLine.y - gradient * pointOnLine.x;

		pointOnLine1 = pointOnLine;
		pointOnLine2 = pointOnLine1 + new Vector2(1, gradient);

		approachSide = false;
		approachSide = GetSide(pointPerpendicularToLine);
	}

	public bool GetSide(Vector2 point)
	{
		return (point.x - pointOnLine1.x) * (pointOnLine2.y - pointOnLine1.y) - (point.y - pointOnLine1.y) * (pointOnLine2.x - pointOnLine1.x) < 0;
	}

	public bool HasCrossedLine(Vector2 point)
	{
		return GetSide(point) != approachSide;
	}

	public void DrawGizmos(float length)
	{
		Vector3 lineDir = new Vector3(1, 0, gradient).normalized;
		Vector3 lineCenter = new Vector3(pointOnLine1.x, 0, pointOnLine1.y) + Vector3.up;
		Gizmos.DrawLine(lineCenter - lineDir * length * .5f, lineCenter + lineDir * length * .5f);
	}
}
