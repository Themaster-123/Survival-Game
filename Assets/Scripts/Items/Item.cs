using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ICloneable
{
	public string name;
	public string description;
	public Sprite sprite;
	public ItemType Type { get; protected set; }

	public Item(ItemSettings itemSettings)
	{
		name = itemSettings.name;
		description = itemSettings.description;
		sprite = itemSettings.sprite;
		Type = itemSettings.itemType;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}


