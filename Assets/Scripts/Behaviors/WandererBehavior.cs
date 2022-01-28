using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFollowerBehavior))]
public class WandererBehavior : Behavior
{
	public float maxWanderTime = 3f;
	public float minWanderTime = 1f;
	public float maxWanderRange = 5f;
	public float minWanderRange = 2f;
	protected PathFollowerBehavior pathFollowerBehavior;
	protected float wanderTime = 0f;

	public virtual void Wander()
	{
		Vector3 wanderPoint = MathUtilities.GenerateRandomPointOnAnnulus(transform.position, minWanderRange, maxWanderRange);
		pathFollowerBehavior.FindPath(wanderPoint);
	}

	protected virtual void Start()
	{
		SetWanderTime();
	}

	protected virtual void FixedUpdate()
	{
		TryWander();
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		pathFollowerBehavior = GetComponent<PathFollowerBehavior>();
	}

	protected virtual void TryWander()
	{
		if (Time.fixedTime >= wanderTime)
		{
			SetWanderTime();
			Wander(); 
		}
	}

	protected virtual float SetWanderTime()
	{
		wanderTime = Time.fixedTime + Random.Range(minWanderTime, maxWanderTime);
		return wanderTime;
	}
}
