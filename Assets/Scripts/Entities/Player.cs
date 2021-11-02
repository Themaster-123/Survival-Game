using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float mouseSensitivity = .1f;
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
        rotation.y = Mathf.Clamp(rotation.y - mouseMovement.y, -90, 90);
		base.MouseRotate(mouseMovement);
    }

	public override void CalculateRotation()
	{
		base.CalculateRotation();
        playerCamera.transform.localRotation = Quaternion.Euler(new Vector3(rotation.y, 0, 0));
    }

	protected override void Awake()
	{
        base.Awake();
        inputMaster = new InputMaster();
        RegisterJump();
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
    }

    protected override void MoveEntity()
	{
        Vector2 movement = GetPlayerMovement();
        Vector2 mouseMovement = GetMouseMovement() * mouseSensitivity;

        Move(movement);
        MouseRotate(mouseMovement);
    }

    protected virtual void RegisterJump()
	{
        inputMaster.Player.Jump.performed += _ => Jump();
	}
}
