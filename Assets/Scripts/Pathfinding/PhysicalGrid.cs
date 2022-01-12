using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalGrid : MonoBehaviour
{
	public World world;
    public Pathfinding pathfinding;

	[Header("testing")]
	public Transform target;
	public Transform seeker;
	public bool calculatePath = false;
	protected Vector3[] path = new Vector3[0];

	protected void Awake()
	{
		InitializeFields();
	}

	protected void OnDrawGizmos()
	{

		DrawGrid();
	}

	protected void OnValidate()
	{
		if (calculatePath == true)
		{
			path = PathRequestManager.GetPath(seeker.position, target.position);
			calculatePath = false;
		}
	}
	protected void InitializeFields()
	{
		pathfinding = new Pathfinding(world);
	}

	protected void DrawGrid()
	{
		Gizmos.color = Color.white;
		Gizmos.matrix = transform.localToWorldMatrix;

		Gizmos.color = Color.cyan;
		Gizmos.matrix = Matrix4x4.identity;
		for (int i = 1; i < path.Length; i++)
		{
			Gizmos.DrawLine(path[i - 1], path[i]);
		}
	}
}
