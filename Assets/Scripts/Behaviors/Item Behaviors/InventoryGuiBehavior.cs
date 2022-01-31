using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryBehavior))]
[RequireComponent(typeof(InputBehavior))]
public class InventoryGuiBehavior : Behavior
{
	public bool Opened 
	{ 
		get
		{
			return inventoryUI.gameObject.activeSelf;
		} 
	}

	public Transform inventoryUI;
	public RectTransform cursorSlot;
	public Item cursorItem;
	protected InventoryBehavior inventoryBehavior;
	protected InputBehavior inputBehavior;
	protected Button[,] slots;
	protected Image[,] slotSprites;
	protected Image cursorSlotSprite;

	public virtual void Open()
	{
		inventoryUI.gameObject.SetActive(true);
		GameUtils.UnlockMouse();
	}

	public virtual void Close()
	{
		inventoryUI.gameObject.SetActive(false);
		GameUtils.LockMouse();
	}

	public virtual void Toggle()
	{
		if (Opened) Close();
		else Open();
	}

	public virtual void OnClickSlot(Vector2Int slot)
	{
		Item oldItem = inventoryBehavior.inventory[slot];
		inventoryBehavior.inventory[slot] = cursorItem;
		cursorItem = oldItem;
	}

	protected virtual void Start()
	{
		GetSlots();
		InitializeCursor();
		Close();
	}

	protected virtual void Update()
	{
		UpdateGui();
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		inventoryBehavior = GetComponent<InventoryBehavior>();
		inputBehavior = GetComponent<InputBehavior>();
	}

	protected virtual void UpdateGui()
	{
		if (!Opened) return;

		for (int x = 0; x < inventoryBehavior.size.x; x++)
		{
			for (int y = 0; y < inventoryBehavior.size.y; y++)
			{
				SetImageToItem(slotSprites[x, y], inventoryBehavior.inventory[x, y]);
			}
		}

		SetImageToItem(cursorSlotSprite, cursorItem);
		cursorSlot.position = inputBehavior.MousePosition;
		GameUtils.ShowCursor(cursorItem == ItemType.Air);
	}

	protected virtual void GetSlots()
	{
		slots = new Button[inventoryBehavior.size.x, inventoryBehavior.size.y];
		slotSprites = new Image[inventoryBehavior.size.x, inventoryBehavior.size.y];
		cursorSlotSprite = cursorSlot.GetComponent<Image>();

		for (int i = 0; i < inventoryBehavior.size.x * inventoryBehavior.size.y; i++)
		{
			int x = i % inventoryBehavior.size.x;
			int y = i / inventoryBehavior.size.x;

			Transform slot = inventoryUI.GetChild(0).GetChild(i);
			slots[x, y] = slot.GetComponent<Button>();
			slotSprites[x, y] = slot.GetChild(0).GetComponent<Image>();
			slots[x, y].onClick.AddListener(() => { OnClickSlot(new Vector2Int(x, y)); });
		}
	}
	
	protected virtual void InitializeCursor()
	{
		cursorItem = ItemDatabase.GetItem(ItemType.Air);
	}

	protected void SetImageToItem(Image image, Item item)
	{
		image.sprite = item.sprite;
		image.color = image.sprite != null ? Color.white : new Color(0, 0, 0, 0);
	}
}
