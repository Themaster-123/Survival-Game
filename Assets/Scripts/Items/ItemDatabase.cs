using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemDatabase : MonoBehaviour
{
	public static ItemDatabase instance;

	[SerializeField]
	protected ItemSettings[] itemDatabase;
	protected Dictionary<ItemType, Item> items;
	protected Dictionary<ItemType, ItemSettings> itemsSettings;

	public static Item GetItem(ItemType itemType)
	{
		return (Item)instance.items[itemType].Clone();
	}

	public static Item GetItem(ItemType itemType, int stackSize)
	{
		Item item = GetItem(itemType);
		item.StackSize = stackSize;
		return item;
	}

	protected void Awake()
	{
		CreateItemSettingsDictionary();
		CreateDatabase();
		SetInstance();
	}

	protected void CreateItemSettingsDictionary()
	{
		itemsSettings = new Dictionary<ItemType, ItemSettings>();

		foreach (ItemSettings itemSettings in itemDatabase)
		{
			itemsSettings.Add(itemSettings.itemType, itemSettings);
		}
	}

	protected void CreateDatabase()
	{
		items = new Dictionary<ItemType, Item>
		{
			{ItemType.Sword, new Item(itemsSettings[ItemType.Sword])},
			{ItemType.Axe, new Item(itemsSettings[ItemType.Axe])},
			{ItemType.Shovel, new Item(itemsSettings[ItemType.Shovel])},
			{ItemType.Air, new Item(itemsSettings[ItemType.Air])},
		};
	}

	protected void SetInstance()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}

[Serializable]
public struct ItemSettings
{
	public ItemType itemType;
	public string name;
	public string description;
	public Sprite sprite;
	public int maxStackSize;
}