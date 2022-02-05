using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour
{
    public GameObject itemBehaviorPrefab;
    protected static GameUtils instance;

    public static void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    public static void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public static void ShowCursor(bool show)
	{
        Cursor.visible = show;
	}

    public static ItemBehavior CreateWorldItem(Item item)
	{
        return instance.InstanceCreateWorldItem(item);
	}

	protected void Awake()
	{
		if (instance == null)
		{
            instance = this;
		}
	}

	protected ItemBehavior InstanceCreateWorldItem(Item item)
	{
        item = (Item)item.Clone();
        item.StackSize = 1;

        GameObject gameObject = Instantiate(itemBehaviorPrefab, Vector3.zero, Quaternion.identity);
        ItemBehavior itemBehavior = gameObject.GetComponent<ItemBehavior>();
        itemBehavior.item = item;
        return itemBehavior;
	}
}
