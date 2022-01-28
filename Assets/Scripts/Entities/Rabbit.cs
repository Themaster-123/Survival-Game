using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehavior))]
[RequireComponent(typeof(DirectionBehavior))]
[RequireComponent(typeof(PathFollowerBehavior))]
[RequireComponent(typeof(WandererBehavior))]
public class Rabbit : Entity
{
	public Transform target; 
	protected MovementBehavior movementBehavior;
	protected DirectionBehavior directionBehavior;
	protected PathFollowerBehavior pathFollowerBehavior;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void MoveEntity()
	{
		base.MoveEntity();
		//movementBehavior.Move(directionBehavior.HorizontalDirection());
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		movementBehavior = GetComponent<MovementBehavior>();
		directionBehavior = GetComponent<DirectionBehavior>();
		pathFollowerBehavior = GetComponent<PathFollowerBehavior>();
	}
}
