using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehavior))]
[RequireComponent(typeof(DirectionBehavior))]
[RequireComponent(typeof(CasterModifierBehavior))]
[AddComponentMenu("Survival Game/Entities/Player")]
public class Player : Entity
{
    public float mouseSensitivity = .1f;
    [Header("Misc Settings")]
    public Camera playerCamera;
    public Voxel modifyVoxel;

    protected InputMaster inputMaster;
    protected MovementBehavior movementBehavior;
    protected DirectionBehavior directionBehavior;
    protected CasterModifierBehavior casterModifierBehavior;

    public virtual Vector2 GetPlayerMovement()
    {
        return inputMaster.Player.Movement.ReadValue<Vector2>();
    }

    public virtual Vector2 GetMouseMovement()
    {
        return inputMaster.Player.MouseMovement.ReadValue<Vector2>();
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
        RegisterInteractInput();
    }

    protected override void Update()
    {
        base.Update();
        CheckIsHoldingJump();
        HandleInput();
    }

    protected override void MoveEntity()
	{
        base.MoveEntity();
    }

    protected virtual void HandleInput()
	{
        Vector2 movement = GetPlayerMovement();
        Vector2 mouseMovement = GetMouseMovement() * mouseSensitivity;

        movementBehavior.Move(movement * Time.deltaTime);
        directionBehavior.Rotate(mouseMovement);
    }

    // jumps if the player is holding jump
    protected virtual void CheckIsHoldingJump()
	{
        if (inputMaster.Player.Jump.ReadValue<float>() > .5f)
		{
            movementBehavior.Jump();
		}
	}

    protected virtual void RegisterInteractInput()
	{
        inputMaster.Player.Interact.performed += context => OnInteract();
        inputMaster.Player.StopInteract.performed += context => OnStopInteract();
    }

	protected override void AddEntityToWorld()
	{
		base.AddEntityToWorld();
        world.AddPlayer(this);
	}

	protected override void RemoveEntityFromWorld()
	{
		base.RemoveEntityFromWorld();
        world.RemovePlayer(this);
	}

    protected virtual void OnInteract()
	{
        casterModifierBehavior.StartContinuousModifying(modifyVoxel);
	}

    protected virtual void OnStopInteract()
	{
        casterModifierBehavior.StopContinuousModifying();
	}

	protected override void GetComponents()
	{
		base.GetComponents();
        directionBehavior = GetComponent<DirectionBehavior>();
        movementBehavior = GetComponent<MovementBehavior>();
        casterModifierBehavior = GetComponent<CasterModifierBehavior>();
	}
}
