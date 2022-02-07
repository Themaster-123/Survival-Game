using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryBehavior))]
[RequireComponent(typeof(InputBehavior))]
[RequireComponent(typeof(InteractableBehavior))]
public class InventoryGuiBehavior : Behavior
{
	public bool Opened 
	{ 
		get
		{
			return inventoryUI.gameObject.activeSelf;
		} 
	}
	public int SelectedHotbarIndex { get; protected set; }
	public Vector2Int SelectedHotbarSlot { get { return new Vector2Int(SelectedHotbarIndex, inventoryBehavior.size.y - 1); } }

	public Transform inventoryUI;
	public Transform hotbarUI;

	public Sprite slotSprite;
	public Sprite slotSelectedSprite;

	public Item cursorItem;

	protected Transform cursorSlot;
	protected InventoryBehavior inventoryBehavior;
	protected InputBehavior inputBehavior;
	protected InteractableBehavior interactableBehavior;
	protected GuiSlot[,] slots;
	protected GuiSlot[] hotbarSlots;
	protected GuiSlot guiCursorSlot;

	public virtual void Open()
	{
		inventoryUI.gameObject.SetActive(true);
		hotbarUI.gameObject.SetActive(false);
		GameUtils.UnlockMouse();
	}

	public virtual void Close()
	{
		inventoryUI.gameObject.SetActive(false);
		hotbarUI.gameObject.SetActive(true);
		GameUtils.LockMouse();
	}

	public virtual void Toggle()
	{
		if (Opened) Close();
		else Open();
	}

	public virtual void OnClickSlot(Vector2Int slot)
	{
		if (!IsInBounds(slot) || !Opened) return;

		if (cursorItem == ItemType.Air)
		{
			Item oldItem = inventoryBehavior.inventory[slot];
			inventoryBehavior.inventory[slot] = cursorItem;
			cursorItem = oldItem;
		} else
		{
			int remaining = inventoryBehavior.inventory.AddItem(slot, cursorItem);
			if (remaining == 0)
			{
				cursorItem = ItemDatabase.GetItem(ItemType.Air);
			} else 
			{ 
				cursorItem.StackSize = remaining;
			}
		}
	}

	public virtual void OnRightClickSlot(Vector2Int slot)
	{
		if (!IsInBounds(slot) || !Opened) return;

		Item slotItem = inventoryBehavior.inventory[slot];
		if (cursorItem == ItemType.Air && slotItem != ItemType.Air)
		{
			int halfStack;
			MathUtils.DivideIntIntoTwoParts(slotItem.StackSize, out _, out halfStack);
			cursorItem = (Item)slotItem.Clone();
			cursorItem.StackSize = halfStack;
			inventoryBehavior.inventory.RemoveItem(slot, halfStack);
		}
		else if (slotItem.StackSize != slotItem.MaxStackSize || slotItem == ItemType.Air)
		{
			if (slotItem == ItemType.Air)
			{
				inventoryBehavior.inventory[slot] = cursorItem;
				inventoryBehavior.inventory[slot].StackSize = 1;
				SubtractOneFromCursorItem();
			} else if (cursorItem.EqualsIgnoreStackSize(slotItem))
			{
				slotItem.StackSize += 1;
				SubtractOneFromCursorItem();
			}
		}		
	}

	public virtual void DropItemInSlot(Vector2Int slot)
	{
		if (IsInBounds(slot) && inventoryBehavior.inventory[slot] != ItemType.Air)
		{
			Item item = inventoryBehavior.inventory[slot];
			inventoryBehavior.inventory.RemoveItem(slot, 1);
			Transform itemBehavior = GameUtils.CreateWorldItem(item).transform;
			itemBehavior.position = GetDropPoint();

		}
	}

	protected virtual void Start()
	{
		GetSlots();
		InitializeCursor();
		Close();
		RegisterInput();
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
		interactableBehavior = GetComponent<InteractableBehavior>();
	}

	protected virtual void UpdateGui()
	{
		if (Opened)
		{
			for (int x = 0; x < inventoryBehavior.size.x; x++)
			{
				for (int y = 0; y < inventoryBehavior.size.y; y++)
				{
					UpdateGuiSlot(slots[x, y], inventoryBehavior.inventory[x, y]);
				}
			}

			UpdateGuiSlot(guiCursorSlot, cursorItem);
			cursorSlot.position = inputBehavior.MousePosition;
			GameUtils.ShowCursor(cursorItem == ItemType.Air);
		} else
		{
			for (int i = 0; i < inventoryBehavior.size.x; i++)
			{
				UpdateGuiSlot(hotbarSlots[i], inventoryBehavior.inventory[i, inventoryBehavior.size.y - 1]);
			}
		}

		UpdateSelectedSlots();
	}

	protected virtual void GetSlots()
	{
		slots = new GuiSlot[inventoryBehavior.size.x, inventoryBehavior.size.y];
		hotbarSlots = new GuiSlot[inventoryBehavior.size.x];
		cursorSlot = inventoryUI.GetChild(1).transform;
		guiCursorSlot = new GuiSlot(cursorSlot.GetChild(0).GetComponent<Image>(), cursorSlot.GetChild(1).GetComponent<TMP_Text>(), null);

		for (int i = 0; i < inventoryBehavior.size.x * inventoryBehavior.size.y; i++)
		{
			Vector2Int slotPos = IndexToSlot(i);

			Transform slot = inventoryUI.GetChild(0).GetChild(i);
			slots[slotPos.x, slotPos.y] = new GuiSlot(slot.GetChild(0).GetComponent<Image>(), slot.GetChild(1).GetComponent<TMP_Text>(), slot.GetComponent<Image>());
		}

		for (int i = 0; i < inventoryBehavior.size.x; i++)
		{
			Transform slot = hotbarUI.GetChild(0).GetChild(i);
			hotbarSlots[i] = new GuiSlot(slot.GetChild(0).GetComponent<Image>(), slot.GetChild(1).GetComponent<TMP_Text>(), slot.GetComponent<Image>());
		}
	}
	
	protected virtual void InitializeCursor()
	{
		cursorItem = ItemDatabase.GetItem(ItemType.Air);
	}

	protected virtual void UpdateGuiSlot(GuiSlot slot, Item item)
	{
		slot.image.sprite = item.sprite;
		slot.image.color = slot.image.sprite != null ? Color.white : new Color(0, 0, 0, 0);

		slot.stackSizeText.text = item.StackSize <= 1 ? "" : item.StackSize.ToString();

		if (slot.slotImage == null) return;

		slot.slotImage.sprite = slotSprite;
	}

	protected virtual void UpdateSelectedSlot(GuiSlot slot, bool selected)
	{
		slot.slotImage.sprite = selected ? slotSelectedSprite : slotSprite;
	}

	// Gets the current slot the mouse is hovering over
	protected virtual Vector2Int GetSelectedSlot()
	{
		if (Opened)
		{
			return GetMouseSlot();
		} else
		{
			return SelectedHotbarSlot;
		}
	}

	protected virtual Vector2Int GetMouseSlot()
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = inputBehavior.MousePosition;
		List<RaycastResult> raysastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, raysastResults);

		for (int i = 0; i < raysastResults.Count; i++)
		{
			RaycastResult result = raysastResults[i];
			if (result.gameObject.tag == "Slot")
			{
				GameObject slot = result.gameObject;
				return IndexToSlot(slot.transform.GetSiblingIndex());
			}
		}

		return -Vector2Int.one;
	}

	protected virtual Vector2Int IndexToSlot(int i)
	{
		int x = i % inventoryBehavior.size.x;
		int y = i / inventoryBehavior.size.x;
		return new Vector2Int(x, y);
	}

	protected virtual void RegisterInput()
	{
		inputBehavior.inputMaster.Player.Interact.performed += context => OnClickSlot(GetSelectedSlot());
		inputBehavior.inputMaster.Player.SecondaryInteract.performed += context => OnRightClickSlot(GetSelectedSlot());
		inputBehavior.inputMaster.Player.Drop.performed += context => DropItemInSlot(GetSelectedSlot());
		inputBehavior.inputMaster.Player.SlotChange.performed += context => ScrollSlot((int)context.ReadValue<float>());
		inputBehavior.inputMaster.Player.InstantSlotChange.performed += context => InstantHotbarChange((int)context.ReadValue<float>() - 1);
	}

	protected virtual bool IsInBounds(Vector2Int slot)
	{
		return !(slot.x < 0 || slot.x >= inventoryBehavior.size.x || slot.y < 0 || slot.y >= inventoryBehavior.size.x);

	}

	protected virtual void SubtractOneFromCursorItem()
	{
		if (cursorItem.StackSize-- - 1 <= 0)
		{
			cursorItem = ItemDatabase.GetItem(ItemType.Air);
		}
	}

	protected virtual void UpdateSelectedSlots()
	{
		Vector2Int mousePos = GetSelectedSlot();
		if (!IsInBounds(mousePos)) return;

		if (Opened)
		{
			UpdateSelectedSlot(slots[mousePos.x, mousePos.y], true);
		} else
		{
			UpdateSelectedSlot(hotbarSlots[mousePos.x], true);
		}
	}

	protected virtual void ScrollSlot(int amount)
	{
		SelectedHotbarIndex = MathUtils.Mod(SelectedHotbarIndex + -amount, inventoryBehavior.size.x);
	}

	protected virtual void InstantHotbarChange(int change)
	{
		if (change < 0) return;
		SelectedHotbarIndex = change;
	}

	protected virtual Vector3 GetDropPoint()
	{
		RaycastHit hit = interactableBehavior.Raycast(~0);
		return hit.point;
	}

	protected struct GuiSlot
	{
		public Image image;
		public TMP_Text stackSizeText;
		public Image slotImage;

		public GuiSlot(Image image, TMP_Text stackSizeText, Image slotImage)
		{
			this.image = image;
			this.stackSizeText = stackSizeText;
			this.slotImage = slotImage;
		}
	}
}
