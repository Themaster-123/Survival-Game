using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Survival Game/Entities/Entity")]
public class Entity : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 8;
    public float acceleration = 8;
    public float deceleration = 8;
    public float jumpStrength = 3;
    public float jumpCooldown = .1f;
    public LayerMask groundLayers;
    public float groundedCheckDistance = .03f;
    public CapsuleCollider enttiyCollider;

    [HideInInspector]
    public Vector2 rotation;

    protected Rigidbody rigidBody;
    protected float jumpTime;

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
        if (IsGrounded() && Time.time >= jumpTime)
		{
            jumpTime = Time.time + jumpCooldown;
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
        Vector3 direction = GetHorizontalEntityRotation() * new Vector3(movement.x, 0, movement.y);
        Vector3 targetVelocity = direction * maxSpeed;
        Vector3 noUpwordVelocity = rigidBody.velocity - Vector3.Scale(rigidBody.velocity, rigidBody.transform.up);
        Vector3 velocityChange = targetVelocity - noUpwordVelocity;

        float maxAccel = Vector3.Dot(velocityChange, noUpwordVelocity.normalized) < 0 ? deceleration : acceleration;

        Vector3 accel = velocityChange;

        accel = Vector3.ClampMagnitude(accel, maxAccel * Time.deltaTime);

        rigidBody.AddForce(accel, ForceMode.VelocityChange);
    }

    // rotates the entity using mouse movements
    public virtual void MouseRotate(Vector2 mouseMovement)
	{
        rotation.x += mouseMovement.x;
        rotation.y = Mathf.Clamp(rotation.y - mouseMovement.y, -90, 90);
        CalculateRotation();
    }

    // calculates rotations off of rotation variable
    public virtual void CalculateRotation()
	{
        transform.localRotation = Quaternion.Euler(new Vector3(0, rotation.x, 0));
	}

    // gets the Horzontal Direction based off of the rotation variable
    public virtual Vector3 HorizontalDirection()
	{
        return transform.TransformDirection(Quaternion.AngleAxis(rotation.x, Vector3.up) * Vector3.forward);
	}

    // gets the rotation based off of the rotation variable
    public virtual Quaternion GetHorizontalEntityRotation()
    {
        return transform.rotation * Quaternion.Euler(0, rotation.x, 0);
    }

    // gets the rotation based off of the rotation variable
    public virtual Quaternion GetEntityRotation()
	{
        return transform.rotation * Quaternion.Euler(rotation.y, rotation.x, 0);
	}

    protected virtual void Awake()
    {
        GetComponents();
    }

    protected virtual void OnEnable()
    {
        jumpTime = Time.time;
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
