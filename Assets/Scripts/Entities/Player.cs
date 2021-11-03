using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Survival Game/Entities/Player")]
public class Player : Entity
{
    public float mouseSensitivity = .1f;
    [Header("Misc Settings")]
    public Camera playerCamera;

    protected InputMaster inputMaster;

    public virtual Vector2 GetPlayerMovement()
    {
        return inputMaster.Player.Movement.ReadValue<Vector2>();
    }

    public virtual Vector2 GetMouseMovement()
    {
        return inputMaster.Player.MouseMovement.ReadValue<Vector2>();
    }

	public override void MouseRotate(Vector2 mouseMovement)
	{
		base.MouseRotate(mouseMovement);
    }


    public override void CalculateRotation()
	{
        playerCamera.transform.localRotation = Quaternion.Euler(new Vector3(rotation.y, rotation.x, 0));
    }

	protected override void Awake()
	{
        base.Awake();
        inputMaster = new InputMaster();
	}

	protected override void OnEnable()
	{
        base.OnEnable();
        inputMaster.Enable();
        LockMouse();
    }

	protected override void OnDisable()
	{
        base.OnDisable();
        inputMaster.Disable();
        UnlockMouse();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        MoveEntity();
        CheckIsHoldingJump();
    }

    protected override void MoveEntity()
	{
        Vector2 movement = GetPlayerMovement();
        Vector2 mouseMovement = GetMouseMovement() * mouseSensitivity;

        Move(movement);
        MouseRotate(mouseMovement);
    }

    // jumps if the player is holding jump
    protected virtual void CheckIsHoldingJump()
	{
        if (inputMaster.Player.Jump.ReadValue<float>() > .5f)
		{
            Jump();
		}
	}
}
