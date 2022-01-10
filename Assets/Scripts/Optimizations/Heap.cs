using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
	public int Count
	{
		get
		{
			return currentItemCount;
		}
	}

	protected T[] items;
	protected int currentItemCount;

	public Heap(int maxHeapSize)
	{
		items = new T[maxHeapSize];
	}

	public void Add(T item)
	{
		item.HeapIndex = currentItemCount;
		items[currentItemCount] = item;
		SortUp(item);
		currentItemCount++;
		if (currentItemCount >= items.Length)
		{
			Array.Resize(ref items, items.Length * 2);
		}
	}

	public T RemoveFirst()
	{
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return firstItem;
	}

	public bool Contains(T item)
	{
		if (item.HeapIndex >= items.Length) return false;

		return Equals(items[item.HeapIndex], item);
	}

	public void UpdateItem(T item)
	{
		SortUp(item);
	}

	protected void SortUp(T item)
	{
		int parentIndex = (item.HeapIndex-1) / 2;

		while (true)
		{
			T parentItem = items[parentIndex];

			if (item.CompareTo(parentItem) > 0)
			{
				Swap(item, parentItem);
			} else
			{
				break;
			}

			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	protected void SortDown(T item)
	{
		while (true)
		{
			int childLeft = item.HeapIndex * 2 + 1;

			if (childLeft < currentItemCount)
			{
				int childRight = item.HeapIndex * 2 + 2;
				int swapIndex = childLeft;


				if (childRight < currentItemCount)
				{
					if (items[childLeft].CompareTo(items[childRight]) < 0)
					{
						swapIndex = childRight;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0)
				{
					Swap(item, items[swapIndex]);
				} else
				{
					return;
				}
			} else
			{
				return;
			}
		}
	}

	protected void Swap(T itemA, T itemB)
	{
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;
		int tempIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = tempIndex;
	}
}

public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex { get; set; }
}
