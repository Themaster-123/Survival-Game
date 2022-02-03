using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
	public Vector2Int Size { get; protected set; }
    protected Item[,] items;

    public Inventory(Vector2Int size) : this(size.x, size.y)
	{
	}

	public Inventory(int width, int height)
	{
		items = new Item[width, height];
		Size = new Vector2Int(width, height);
		ClearInventory();
	}

	public Item GetItem(int x, int y)
	{
		return items[x, y];
	}

	public Item GetItem(Vector2Int pos)
	{
		return GetItem(pos.x, pos.y); 
	}

	public void SetItem(int x, int y, Item item)
	{
		items[x, y] = (Item)item.Clone();
	}

	public void SetItem(Vector2Int pos, Item item)
	{
		SetItem(pos.x, pos.y, item);
	}

	public void ClearInventory()
	{
		for (int x = 0; x < Size.x; x++)
		{
			for (int y = 0; y < Size.y; y++)
			{
				items[x, y] = ItemDatabase.GetItem(ItemType.Air);
			}
		}
	}

	// adds the item to the slot and returns the remaining stack size
	public int AddItem(Vector2Int pos, Item item)
	{
		Item posItem = GetItem(pos);
		if (item != ItemType.Air && (posItem == ItemType.Air || posItem.EqualsIgnoreStackSize(item)))
		{
			int oldStackSize = posItem.StackSize;
			if (posItem == ItemType.Air)
			{
				posItem = (Item)item.Clone();
				posItem.StackSize = 1;
				oldStackSize = 0;
			}
			posItem.StackSize = oldStackSize + item.StackSize;
			SetItem(pos, posItem);
			return Mathf.Max((oldStackSize + item.StackSize) - item.MaxStackSize, 0);
		}

		return item.StackSize;
	}

	public void RemoveItem(Vector2Int pos, int amount)
	{
		Item posItem = GetItem(pos);
		int oldStackSize = posItem.StackSize;
		posItem.StackSize -= amount;
		if (oldStackSize - amount <= 0)
		{
			SetItem(pos, ItemDatabase.GetItem(ItemType.Air));
		}
	}

	public Item this[int x, int y]
	{
		get
		{
			return GetItem(x, y);
		}

		set
		{
			SetItem(x, y, value);
		}
	}

	public Item this[Vector2Int pos]
	{
		get
		{
			return GetItem(pos);
		}

		set
		{
			SetItem(pos, value);
		}
	}
}
