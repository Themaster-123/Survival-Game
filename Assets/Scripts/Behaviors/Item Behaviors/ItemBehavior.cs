using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : Behavior 
{
    public Item item;
    protected MeshFilter meshFilter;
	protected MeshCollider meshCollider;

	protected override void Awake()
	{
		base.Awake();
	}

	protected virtual void Start()
	{
		SetMeshFilterToItemMesh();
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		meshFilter = GetComponent<MeshFilter>();
		meshCollider = GetComponent<MeshCollider>();
	}

	protected void SetMeshFilterToItemMesh()
	{
		meshFilter.mesh = item.mesh;
		meshCollider.sharedMesh = item.mesh;
	}
}
