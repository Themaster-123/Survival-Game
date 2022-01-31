using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBehavior : Behavior
{
    public Inventory inventory;
    public Vector2Int size;

	protected override void GetComponents()
	{
		base.GetComponents();
		CreateInventory();
	}

	protected void CreateInventory()
	{
		inventory = new Inventory(size);
	}
}
