using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ICloneable, IEquatable<Item>, IEquatable<ItemType>
{
	public int StackSize
	{
		get
		{
			return stackSize;
		}

		set
		{
			stackSize = Math.Max(Math.Min(value, MaxStackSize), 1);
		}
	}

	public ItemType Type { get; protected set; }
	public int MaxStackSize { get; protected set; }

	public string name;
	public string description;
	public Sprite sprite;
	public Mesh mesh;

	protected int stackSize;
	protected ItemSettings itemSettings;

	public Item(ItemSettings itemSettings)
	{
		name = itemSettings.name;
		description = itemSettings.description;
		sprite = itemSettings.sprite;
		Type = itemSettings.itemType;
		MaxStackSize = itemSettings.maxStackSize;
		mesh = itemSettings.mesh;
		StackSize = 1;
		this.itemSettings = itemSettings;
	}

	public virtual void Use(ItemBehavior itemBehavior, GameObject caller)
	{
		MonoBehaviour.print(itemBehavior.transform.position);
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public bool Equals(Item other)
	{
		return EqualsIgnoreStackSize(other) && StackSize == other.StackSize;
	}

	public bool EqualsIgnoreStackSize(Item other)
	{
		return !(other is null) && Type == other.Type && name == other.name && description == other.description;
	}

	public bool Equals(ItemType other)
	{
		return Type == other;
	}

	public override bool Equals(object other)
	{
		if (typeof(Item).IsInstanceOfType(other))
		{
			return Equals((Item)other);
		}
		else if (typeof(ItemType).IsInstanceOfType(other))
		{
			return Equals((ItemType)other);
		}

		return false;
	}

	public override int GetHashCode()
	{
		int hashCode = -1294763954;
		hashCode = hashCode * -1521134295 + StackSize.GetHashCode();
		hashCode = hashCode * -1521134295 + Type.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(description);
		hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(sprite);
		return hashCode;
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


