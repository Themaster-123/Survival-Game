using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBehavior : Behavior
{
    public InputMaster inputMaster;
    public Vector2 MousePosition
	{
        get
		{
            return inputMaster.Player.UIMousePosition.ReadValue<Vector2>();
		}
	}

    protected override void Awake()
    {
        base.Awake();
        inputMaster = new InputMaster();
    }

    protected virtual void OnEnable()
    {
        EnableInputMaster();
    }

    protected virtual void OnDisable()
    {
        DisableInputMaster();
    }

    protected virtual void InitializeInputMaster()
	{
        inputMaster = new InputMaster();
    }

    protected virtual void EnableInputMaster()
	{
        inputMaster.Enable();
    }

    protected virtual void DisableInputMaster()
    {
        inputMaster.Enable();
    }
}
