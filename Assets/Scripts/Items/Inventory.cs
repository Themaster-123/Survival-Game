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
		items[x, y] = item;
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
