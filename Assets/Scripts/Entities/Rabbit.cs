using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehavior))]
[RequireComponent(typeof(DirectionBehavior))]
public class Rabbit : Entity
{
	protected MovementBehavior movementBehavior;
	protected DirectionBehavior directionBehavior;

	protected override void MoveEntity()
	{
		base.MoveEntity();
		movementBehavior.Move(directionBehavior.HorizontalDirection());
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		movementBehavior = GetComponent<MovementBehavior>();
		directionBehavior = GetComponent<DirectionBehavior>();
	}
}
