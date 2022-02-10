using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHoldingBehavior : Behavior
{
	public Item CurrentHeldItem
	{
		get
		{
			return currentHeldItemObject.item;
		}
	}

    public Transform itemHoldingTransform;
	protected ItemBehavior currentHeldItemObject;

    public void HoldItem(Item item)
	{
		if (currentHeldItemObject != null)
		{
			Destroy(currentHeldItemObject.gameObject);
		}

		currentHeldItemObject = GameUtils.CreateWorldItem(item, false);
		currentHeldItemObject.transform.parent = itemHoldingTransform;
		currentHeldItemObject.transform.localPosition = Vector3.zero;
		currentHeldItemObject.transform.localRotation = Quaternion.identity;
	}

	public void UseItem()
	{
		CurrentHeldItem.Use(currentHeldItemObject, gameObject);
	}
}
