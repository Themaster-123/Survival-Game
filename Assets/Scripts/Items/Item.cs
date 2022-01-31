using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ICloneable, IEquatable<Item>, IEquatable<ItemType>
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

	public bool Equals(Item other)
	{
		return Type == other.Type && name == other.name && description == other.description;
	}

	public bool Equals(ItemType other)
	{
		return Type == other;
	}

	public static bool operator ==(Item a, Item b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(Item a, Item b)
	{
		return !a.Equals(b);
	}

	public static bool operator ==(Item a, ItemType b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(Item a, ItemType b)
	{
		return !a.Equals(b);
	}
}


