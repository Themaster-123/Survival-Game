using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehavior))]
[RequireComponent(typeof(DirectionBehavior))]
[RequireComponent(typeof(CasterModifierBehavior))]
[RequireComponent(typeof(InventoryGuiBehavior))]
[RequireComponent(typeof(ItemCollector))]
[RequireComponent(typeof(HotbarHoldingBehavior))]
[RequireComponent(typeof(RotatorBehavior))]
[AddComponentMenu("Survival Game/Entities/Player")]
public class Player : Entity
{
    public float mouseSensitivity = .1f;
    [Header("Misc Settings")]
    public Camera playerCamera;
    public Voxel modifyVoxel;
    [HideInInspector]
    public bool stopInput = false;

    protected MovementBehavior movementBehavior;
    protected DirectionBehavior directionBehavior;
    protected CasterModifierBehavior casterModifierBehavior;
    protected InventoryBehavior inventoryBehavior;
    protected InventoryGuiBehavior inventoryGuiBehavior;
    protected ItemHoldingBehavior itemHoldingBehavior;
    protected InputBehavior inputBehavior;

    public virtual Vector2 GetPlayerMovement()
    {
        return inputBehavior.inputMaster.Player.Movement.ReadValue<Vector2>();
    }

    public virtual Vector2 GetMouseMovement()
    {
        return inputBehavior.inputMaster.Player.MouseMovement.ReadValue<Vector2>();
    }

	protected override void Awake()
	{
        base.Awake();
	}

	protected override void OnEnable()
	{
        base.OnEnable();
        GameUtils.LockMouse();
    }

	protected override void OnDisable()
	{
        base.OnDisable();
        GameUtils.UnlockMouse();
    }

    protected override void Start()
    {
        base.Start();
        RegisterInteractInput();
        inventoryBehavior.inventory[0, 0] = ItemDatabase.GetItem(ItemType.Axe, 1);
        inventoryBehavior.inventory[1, 0] = ItemDatabase.GetItem(ItemType.Axe, 1);
        inventoryBehavior.inventory[2, 0] = ItemDatabase.GetItem(ItemType.Axe, 1);
        inventoryBehavior.inventory[0, 1] = ItemDatabase.GetItem(ItemType.Sword);
        inventoryBehavior.inventory[0, 2] = ItemDatabase.GetItem(ItemType.Shovel, 64);
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
        if (stopInput) return;

        Vector2 movement = GetPlayerMovement();
        Vector2 mouseMovement = GetMouseMovement() * mouseSensitivity;

        movementBehavior.Move(movement * Time.deltaTime);
        directionBehavior.Rotate(mouseMovement);
    }

    // jumps if the player is holding jump
    protected virtual void CheckIsHoldingJump()
	{
        if (stopInput) return;

        if (inputBehavior.inputMaster.Player.Jump.ReadValue<float>() > .5f)
		{
            movementBehavior.Jump();
		}
	}

    protected virtual void RegisterInteractInput()
	{
        inputBehavior.inputMaster.Player.Interact.performed += context => OnInteract();
        inputBehavior.inputMaster.Player.Interact.performed += context => itemHoldingBehavior.UseItem();
        inputBehavior.inputMaster.Player.StopInteract.performed += context => OnStopInteract();
        inputBehavior.inputMaster.Player.ToggleInventory.performed += context => ToggleInventory();
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
        if (stopInput) return;

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
        inventoryBehavior = GetComponent<InventoryBehavior>();
        inventoryGuiBehavior = GetComponent<InventoryGuiBehavior>();
        inputBehavior = GetComponent<InputBehavior>();
        itemHoldingBehavior = GetComponent<ItemHoldingBehavior>();
    }

    protected virtual void ToggleInventory()
	{
        inventoryGuiBehavior.Toggle(); 
        stopInput = !stopInput;
        OnStopInteract();
    }
}
 