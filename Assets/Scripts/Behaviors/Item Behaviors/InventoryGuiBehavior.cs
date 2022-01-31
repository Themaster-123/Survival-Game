using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryBehavior))]
public class InventoryGuiBehavior : Behavior
{
	public Transform inventoryUI;
	protected InventoryBehavior inventoryBehavior;
	protected Button[,] slots;
	protected Image[,] slotSprites;

	protected virtual void Start()
	{
		GetSlots();
	}

	protected virtual void Update()
	{
		UpdateGui();
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		inventoryBehavior = GetComponent<InventoryBehavior>();
	}

	protected virtual void UpdateGui()
	{
		for (int x = 0; x < inventoryBehavior.size.x; x++)
		{
			for (int y = 0; y < inventoryBehavior.size.y; y++)
			{
				slotSprites[x, y].sprite = inventoryBehavior.inventory[x, y].sprite;
				slotSprites[x, y].color = slotSprites[x, y].sprite != null ? Color.white : new Color(0, 0, 0, 0);
			}
		}
	}

	protected virtual void GetSlots()
	{
		slots = new Button[inventoryBehavior.size.x, inventoryBehavior.size.y];
		slotSprites = new Image[inventoryBehavior.size.x, inventoryBehavior.size.y];

		for (int i = 0; i < inventoryBehavior.size.x * inventoryBehavior.size.y; i++)
		{
			int x = i % inventoryBehavior.size.x;
			int y = i / inventoryBehavior.size.x;

			Transform slot = inventoryUI.GetChild(i);
			slots[x, y] = slot.GetComponent<Button>();
			slotSprites[x, y] = slot.GetChild(0).GetComponent<Image>();
		}
	}
}
