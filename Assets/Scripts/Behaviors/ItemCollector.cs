using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableBehavior))]
[RequireComponent(typeof(InputBehavior))]
public class ItemCollector : Behavior
{
	public LayerMask itemMask;
	protected InteractableBehavior interactableBehavior;
	protected InputBehavior inputBehavior;
	protected InventoryBehavior inventoryBehavior;

	public void RegisterInput()
	{
		inputBehavior.inputMaster.Player.Interact.performed += context => CollectItem();
	}

	public void CollectItem()
	{
		RaycastHit raycastHit = interactableBehavior.Raycast(itemMask);
		if (raycastHit.collider != null)
		{
			inventoryBehavior.inventory.AddItem(raycastHit.transform.gameObject.GetComponent<ItemBehavior>().item);
			Destroy(raycastHit.transform.gameObject);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		RegisterInput();
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		interactableBehavior = GetComponent<InteractableBehavior>();
		inputBehavior = GetComponent<InputBehavior>();
		inventoryBehavior = GetComponent<InventoryBehavior>();
	}
}
