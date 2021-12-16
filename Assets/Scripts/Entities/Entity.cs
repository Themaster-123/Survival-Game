using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Survival Game/Entities/Entity")]
public class Entity : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 8;
    public float acceleration = 80;
    public float maxAirSpeed = 4;
    public float airAcceleration = 20;
    public float maxSlopeAngle = 65;
    public float snapPredictionTime = .1f;
    [HideInInspector]
    public Vector2 rotation;

    [Header("Jump Settings")]
    public float jumpStrength = 3;
    public float jumpCooldown = .1f;
    public LayerMask groundLayers;
    public float groundedCheckDistance = .03f; 

    [Header("Interaction")]
    public Vector3 headPosition;
    public Vector3 feetPosition;
    public float groundCheckRadius = .9f;
    public float maxInteractionDistance = 5;
    public LayerMask interactionMask;
    public LayerMask terrainMask;
    public SquareStencil stencil;

    [Header("Misc")]
    public World world;

    protected Rigidbody rigidBody;
    protected float jumpTime;
    // used to move in FixedUpdate
    protected Vector2 physicsMovement;
    protected bool jumped;
    // tracks if entity is on ground
    protected bool onGround = false;
    protected uint stepsSinceLastGrounded = 0;
    protected uint stepsSinceLastJump = 0;
    protected Vector3 contactNormal;
    protected Vector3 combinedNormals;

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
            jumped = true;
            jumpTime = Time.time + jumpCooldown;
		}
    }

    public bool IsGrounded()
	{
        /*return IsGrounded(out _);*/
        return onGround;
	}

    // changes physicsMovement to the sum of all movement
    public virtual void Move(Vector2 movement)
    {
        physicsMovement += movement;
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

    public virtual Vector3 GetDirection()
	{
        return transform.TransformDirection(GetEntityRotation() * Vector3.forward);
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

    // converts position to Chunk Position
    public virtual Vector3Int GetChunkPosition()
	{
        return ChunkPositionUtilities.ToChunkPosition(transform.position, world);
    }

    protected virtual void Awake()
    {
        GetComponents();
    }

    protected virtual void OnEnable()
    {
        jumpTime = Time.time;
        AddEntityToWorld();
    }

    protected virtual void OnDisable()
    {
        RemoveEntityFromWorld();
    }


    protected virtual void Start()
    {        
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
	{
        UpdateState();
        IncrementStepsSinceLastGrounded();
        SnapToGround();
        PhysicsMove();
        print(contactNormal);

        ClearState();
    }

    protected void OnCollisionEnter(Collision collision)
	{
        CheckIfOnGround(collision);
    }

	protected void OnCollisionStay(Collision collision)
	{
        CheckIfOnGround(collision);
    }

	// handles ai / input
	protected virtual void MoveEntity()
    {
    }

    // changes the entity velocity to the target velocity(movement) then resets physicsMovement
    protected virtual void PhysicsMove()
    {
		physicsMovement.Normalize();

        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;
        
        Vector3 direction = GetHorizontalEntityRotation() * new Vector3(physicsMovement.x, 0, physicsMovement.y);

        Vector3 horizontalVel = new Vector3(Vector3.Dot(rigidBody.velocity, xAxis), 0, Vector3.Dot(rigidBody.velocity, zAxis));
        Vector3 targetVelocity = direction * (IsGrounded() ? maxSpeed : maxAirSpeed);
        Vector3 velocityDifference = targetVelocity - horizontalVel;

        Vector3 force =  Vector3.ClampMagnitude(ProjectOnContactPlane(velocityDifference), (IsGrounded() ? acceleration : airAcceleration) * Time.fixedDeltaTime);

        rigidBody.AddForce(force, ForceMode.VelocityChange);
        physicsMovement = Vector2.zero;

        if (jumped)
		{
            jumped = false;
            PhysicsJump();
		}
    }

    protected virtual void PhysicsJump()
	{
        stepsSinceLastJump = 0;
        rigidBody.AddForce(transform.up * jumpStrength, ForceMode.VelocityChange);
    }

    protected virtual void GetComponents()
	{
        rigidBody = GetComponent<Rigidbody>();
    }

    // adds the entity to entites list in the current world
    protected virtual void AddEntityToWorld()
	{
        world.AddEntity(this);
	}

    protected virtual void RemoveEntityFromWorld()
    {
        world.RemoveEntity(this);
    }

    protected virtual RaycastHit RaycastFromHead(LayerMask mask)
	{
        Vector3 origin = transform.TransformPoint(GetHorizontalEntityRotation() * headPosition);
        Vector3 direction = GetDirection();

		Physics.Raycast(origin, direction, out RaycastHit hit, maxInteractionDistance, mask, QueryTriggerInteraction.Ignore);

		return hit;
	}

    protected virtual RaycastHit RaycastFromHead()
    {
        return RaycastFromHead(interactionMask);
    }

    protected virtual void AttemptDig()
	{
        RaycastHit hit = RaycastFromHead(terrainMask);

        if (hit.collider != null)
		{
            Dig(hit.point);
		}
	}

    // digs into terrain with the shape of the stencil
    protected virtual void Dig(Vector3 position)
	{
        Vector3Int voxelPosition = VoxelUtilities.ToVoxelPosition(position, world);
        print(voxelPosition);
        stencil.SetVoxel(new Voxel(1), voxelPosition, world);
	}

    protected virtual void SetGrounded(bool grounded, Vector3 normal)
	{
        onGround = grounded;
        contactNormal = normal.normalized;
	}

    protected virtual void SetGrounded(bool grounded)
    {
        SetGrounded(grounded, Vector3.up);
    }

    // checks if the ground slope is below or equal maxSlopeAngle and other stuff
    protected virtual void CheckIfOnGround(Collision collision)
	{
        for (int i = 0; i < collision.contactCount; i++)
		{
            if (IsNormalBelowSlopeAngle(collision.contacts[i].normal))
			{
                combinedNormals += collision.contacts[i].normal;
			}
		}

        if (combinedNormals != Vector3.zero)
		{
            SetGrounded(true);
            return;
        }

        SetGrounded(false);
	}

    protected virtual void IncrementStepsSinceLastGrounded()
	{
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;
	}

    protected bool PredictIfGroundUnderEntity(out RaycastHit hitPoint)
    {
        Vector3 p1 = transform.position + feetPosition + rigidBody.velocity * snapPredictionTime;

        RaycastHit[] hits = Physics.RaycastAll(p1, -transform.up, groundedCheckDistance, groundLayers, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
        {
            if (((groundLayers.value >> hit.transform.gameObject.layer) & 1) == 1 && hit.transform.gameObject != gameObject)
            {
                hitPoint = hit;
                return true;
            }
        }
        RaycastHit nullHitPoint = new RaycastHit();
        hitPoint = nullHitPoint;
        return false;
    }

    protected virtual void SnapToGround()
	{
        if (IsGrounded()) {
            stepsSinceLastGrounded = 0;
            return;
		}

        if (stepsSinceLastGrounded == 1 && stepsSinceLastJump > 2)
		{
            bool groundUnder = PredictIfGroundUnderEntity(out RaycastHit hit);
            print(groundUnder);

            if (groundUnder && IsNormalBelowSlopeAngle(hit.normal))
            {
                print(hit.normal);
                SetGrounded(true, hit.normal);
				float speed = rigidBody.velocity.magnitude;
				float dot = Vector3.Dot(rigidBody.velocity, hit.normal);
				rigidBody.velocity = (rigidBody.velocity - hit.normal * dot).normalized * speed;
			}
        }
	}

    protected virtual bool IsNormalBelowSlopeAngle(Vector3 normal)
	{
        return Vector3.Angle(Vector3.up, normal) <= maxSlopeAngle;
    }

    Vector3 ProjectOnContactPlane(Vector3 vector)
	{
        return Vector3.ProjectOnPlane(vector, contactNormal);
	}

    protected void ClearState()
	{
        SetGrounded(false);
        combinedNormals = Vector3.zero;
	}

    protected void UpdateState()
	{
        SetGrounded(onGround, combinedNormals);
	}
}
