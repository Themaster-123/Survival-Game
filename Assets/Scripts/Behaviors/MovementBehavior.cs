using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionBehavior))]
public class MovementBehavior : Behavior
{
    [Header("Movement Settings")]
    public float maxSpeed = 8;
    public float acceleration = 80;
    public float maxAirSpeed = 8;
    public float airAcceleration = 10;
    public float maxSlopeAngle = 65;
    public float snapPredictionTime = .1f;

    [Header("Jump Settings")]
    public float jumpStrength = 3;
    public float jumpCooldown = .1f;
    public LayerMask groundLayers;
    public float groundedCheckDistance = 2.0f;

    protected Rigidbody rigidBody;
    // tracks if entity is on ground
    protected bool onGround = false;
    protected uint stepsSinceLastGrounded = 0;
    protected uint stepsSinceLastJump = 0;
    protected Vector3 contactNormal;
    protected Vector3 combinedNormals;
    // used to move in FixedUpdate
    protected Vector2 physicsMovement;
    protected float jumpTime;
    protected bool jumped;
    protected DirectionBehavior directionBehavior;

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

    // makes the rigidbody jump with the force of jumpStrength
    public virtual void Jump()
    {
        if (IsGrounded() && Time.time >= jumpTime)
        {
            jumped = true;
            jumpTime = Time.time + jumpCooldown;
        }
    }

    // changes the rigidbody velocity to the target velocity(movement) then resets physicsMovement
    protected virtual void PhysicsMove()
    {
        physicsMovement.Normalize();

        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        Vector3 direction = directionBehavior.GetHorizontalEntityRotation() * new Vector3(physicsMovement.x, 0, physicsMovement.y);

        Vector3 horizontalVel = new Vector3(Vector3.Dot(rigidBody.velocity, xAxis), 0, Vector3.Dot(rigidBody.velocity, zAxis));
        Vector3 targetVelocity = direction * (IsGrounded() ? maxSpeed : maxAirSpeed);
        Vector3 velocityDifference = targetVelocity - horizontalVel;

        Vector3 force = Vector3.ClampMagnitude(ProjectOnContactPlane(velocityDifference), (IsGrounded() ? acceleration : airAcceleration) * Time.fixedDeltaTime);

        rigidBody.AddForce(force, ForceMode.VelocityChange);
        physicsMovement = Vector2.zero;


    }

    protected virtual void OnEnable()
	{
        SetJumpTime();
    }

    protected override void Awake()
	{
        base.Awake();
	}

    protected virtual void FixedUpdate()
    {
        UpdateState();
        IncrementStepsSinceLastGrounded();
        SnapToGround();
        PhysicsMove();
        PhysicsJump();
        PreventSlipping();

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
        Vector3 p1 = transform.position + rigidBody.velocity * snapPredictionTime;

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
        if (IsGrounded())
        {
            stepsSinceLastGrounded = 0;
            return;
        }

        if (stepsSinceLastGrounded == 1 && stepsSinceLastJump > 2)
        {
            bool groundUnder = PredictIfGroundUnderEntity(out RaycastHit hit);

            if (groundUnder && IsNormalBelowSlopeAngle(hit.normal))
            {
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

    protected virtual void PhysicsJump()
    {
        if (jumped)
        {
            jumped = false;
            stepsSinceLastJump = 0;
            rigidBody.AddForce(transform.up * jumpStrength, ForceMode.VelocityChange);
        }
    }

    protected virtual void SetJumpTime()
	{
        jumpTime = Time.time;
    }

    // Stops the rigidbody from slipping on a slope
    protected virtual void PreventSlipping()
	{
        if (!onGround) return;

        //Vector3.Dot(-Physics.gravity, contactNormal)
        // * (Physics.gravity.magnitude * (Vector3.Angle(-Physics.gravity.normalized, contactNormal) / 180))
        Vector3 slipVelocity = (contactNormal - (-Physics.gravity.normalized)) * Physics.gravity.magnitude;
        print(slipVelocity);

        rigidBody.AddForce(-slipVelocity, ForceMode.Acceleration);
	}

    protected override void GetComponents()
    {
        rigidBody = GetComponent<Rigidbody>();
        directionBehavior = GetComponent<DirectionBehavior>();
    }
}
