using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float maxSpeed = 8;
    public float acceleration = 8;
    public float deceleration = 8;
    public float jumpStrength = 3;
    public LayerMask groundLayers;
    public float groundedCheckDistance = .03f;
    public CapsuleCollider enttiyCollider;

    [HideInInspector]
    public Vector2 rotation;

    protected Rigidbody rigidBody;

    public virtual void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    public virtual void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;

    }

    // makes the entity jump with the force of jumpStrength
    public virtual void Jump()
    {
        print("asd");
        if (IsGrounded())
		{
            print("fff");
            rigidBody.AddForce(transform.up * jumpStrength, ForceMode.VelocityChange);
		}
    }

    public bool IsGrounded()
	{
        Vector3 p1 = enttiyCollider.transform.position + enttiyCollider.center + enttiyCollider.transform.up * (-enttiyCollider.height + enttiyCollider.radius * 2) * 0.5F;
        RaycastHit[] hits = Physics.SphereCastAll(p1, enttiyCollider.radius, -enttiyCollider.transform.up, groundedCheckDistance, groundLayers, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
		{
            if (((groundLayers.value >> hit.transform.gameObject.layer) & 1) == 1 && hit.transform.gameObject != gameObject)
			{
                return true;
			}
		}

        return false;
	}

    // changes the entity velocity to the target velocity(movement)
    public virtual void Move(Vector2 movement)
    {
        Vector3 direction = rigidBody.transform.rotation * new Vector3(movement.x, 0, movement.y);
        Vector3 targetVelocity = direction * maxSpeed;
        Vector3 noUpwordVelocity = rigidBody.velocity - Vector3.Scale(rigidBody.velocity, rigidBody.transform.up);
        Vector3 velocityChange = targetVelocity - noUpwordVelocity;

        float maxAccel = Vector3.Dot(velocityChange, noUpwordVelocity.normalized) < 0 ? deceleration : acceleration;

        Vector3 accel = velocityChange;

        accel = Vector3.ClampMagnitude(accel, maxAccel);

        rigidBody.AddForce(accel, ForceMode.Acceleration);
    }

    // rotates the entity using mouse movements
    public virtual void MouseRotate(Vector2 mouseMovement)
	{
        rotation.x += mouseMovement.x;
        CalculateRotation();
    }

    // calculates rotations off of rotation variable
    public virtual void CalculateRotation()
	{
        transform.localRotation = Quaternion.Euler(new Vector3(0, rotation.x, 0));
	}

    protected virtual void Awake()
    {
        GetComponents();
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }


    protected virtual void Start()
    {        
    }

    protected virtual void Update()
    {     
    }

    // handles ai / input
    protected virtual void MoveEntity()
    {
    }

    protected virtual void GetComponents()
	{
        rigidBody = GetComponent<Rigidbody>();
    }
}
