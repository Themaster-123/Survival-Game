using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalGrid : MonoBehaviour
{
	public World World 
	{ 
		get 
		{ 
			lock (world) 
			{ 
				return world; 
			} 
		} 
		set 
		{ 
			lock (world) 
			{ 
				pathfinding.world = value; 
				world = value; 
			} 
		} 
	}

    public Pathfinding pathfinding;

	[Header("testing")]
	public Transform target;
	public Transform seeker;
	protected Vector3[] path = new Vector3[0];
	[SerializeField]
	protected World world;

	protected void Awake()
	{
		InitializeFields();
	}

	protected void OnDrawGizmos()
	{
		DrawGrid();
	}

	protected void InitializeFields()
	{
		pathfinding = new Pathfinding(World);
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

	protected void GetPath()
	{
		PathRequestManager.GetPath(seeker.position, target.position, 65, (Vector3[] path) => { this.path = path; GetPath(); });

	}
}
