using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float maxSpeed = 5;
    public float acceleration = 1;
    public float deceleration = 1;
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

    protected override void Awake()
	{
        inputMaster = new InputMaster();
        rigidBody = GetComponent<Rigidbody>();
	}

	protected override void OnEnable()
	{
        inputMaster.Enable();
        LockMouse();

    }

	protected override void OnDisable()
	{
        inputMaster.Disable();
        UnlockMouse();
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        MoveEntity();
    }

    protected override void MoveEntity()
	{
        Vector2 movement = GetPlayerMovement();

        rigidBody.AddForce(new Vector3(movement.x, 0, movement.y), ForceMode.Acceleration);

        Vector2 mouseMovement = GetMouseMovement() * mouseSensitivity;

        playerCamera.transform.localRotation = playerCamera.transform.localRotation * Quaternion.AngleAxis(-mouseMovement.y, Vector3.right);
        transform.rotation = transform.rotation * Quaternion.AngleAxis(mouseMovement.x, transform.up);
    }
}
