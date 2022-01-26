using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MovementBehavior))]
public class PathFollowerBehavior : Behavior
{
	public float maxPathDistance = 1f;
	[Range(0, 100)]
	public float steeringStrength = 25f;
	public float maxSlowDownDistance = 1f;
	public float minSlowDownDistance = .5f;

	public Vector3[] CurrentPath { get; protected set; }
	protected MovementBehavior movementBehavior;
	protected int currentPathIndex = 0;
	protected bool findingPath = false;
	protected Rigidbody rigidBody;

	public virtual void FindPath(Vector3 end)
	{
		if (findingPath) return;

		findingPath = true;
		PathRequestManager.GetPath(transform.position, end, movementBehavior.maxSlopeAngle, (Vector3[] path) => { CurrentPath = path; currentPathIndex = 0; findingPath = false; });
	}

	protected virtual void FixedUpdate()
	{
		FollowPath();
	}

	protected virtual void FollowPath()
	{
		if (CurrentPath == null || CurrentPath.Length == 0) return;

		if (currentPathIndex < CurrentPath.Length - 1 && (MathUtilities.Flatten(CurrentPath[currentPathIndex]) - MathUtilities.Flatten(transform.position)).sqrMagnitude < maxPathDistance * maxPathDistance)
		{
			currentPathIndex++;
		}

		Vector2 offset = MathUtilities.Flatten(CurrentPath[currentPathIndex]) - MathUtilities.Flatten(transform.position);
		Vector2 direction = offset.normalized;

		Vector3 desiredVelocity = MathUtilities.UnFlatten(direction);
		Vector3 movementVelocity = movementBehavior.MovementVelocity;
		Vector3 steering = desiredVelocity - movementVelocity;
		steering *= steeringStrength * 0.01f;
		float speed = 1;

		if (currentPathIndex == CurrentPath.Length - 1)
		{
			if (offset.sqrMagnitude <= maxSlowDownDistance * maxSlowDownDistance)
			{
				float clampedDistance = Mathf.Clamp(offset.magnitude, minSlowDownDistance, maxSlowDownDistance);
				speed = MathUtilities.MapToRange(clampedDistance, minSlowDownDistance, maxSlowDownDistance, 0f, 1f);
			}
		}

		movementBehavior.Move((movementVelocity + steering) * speed * Time.fixedDeltaTime);
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		movementBehavior = GetComponent<MovementBehavior>();
		rigidBody = GetComponent<Rigidbody>();
	}
}
