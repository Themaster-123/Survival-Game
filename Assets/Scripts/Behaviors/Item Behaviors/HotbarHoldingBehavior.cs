using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemHoldingBehavior))]
[RequireComponent(typeof(InventoryGuiBehavior))]
[RequireComponent(typeof(InventoryBehavior))]
public class HotbarHoldingBehavior : Behavior
{
    protected ItemHoldingBehavior itemHoldingBehavior;
	protected InventoryGuiBehavior inventoryGuiBehavior;
	protected Item prevItem;

	protected virtual void Update()
	{
		UpdateHeldItem();
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		itemHoldingBehavior = GetComponent<ItemHoldingBehavior>();
		inventoryGuiBehavior = GetComponent<InventoryGuiBehavior>();
		
	}

	protected virtual void UpdateHeldItem()
	{
		if (!inventoryGuiBehavior.CurrentSelectedHotbarItem.EqualsIgnoreStackSize(prevItem))
		{
			itemHoldingBehavior.HoldItem(inventoryGuiBehavior.CurrentSelectedHotbarItem);
			prevItem = inventoryGuiBehavior.CurrentSelectedHotbarItem;
		}
	}
}
