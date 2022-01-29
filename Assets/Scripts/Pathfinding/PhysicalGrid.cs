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

	[SerializeField]
	protected World world;

	protected void Awake()
	{
		InitializeFields();
	}

	protected void InitializeFields()
	{
		pathfinding = new Pathfinding(World);
	}
}
