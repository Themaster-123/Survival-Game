using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    public static void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
