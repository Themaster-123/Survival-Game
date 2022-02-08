using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionBehavior))]
public class RotatorBehavior : Behavior
{
	public Transform rotationTransform;
	protected DirectionBehavior directionBehavior;

	protected virtual void LateUpdate()
	{
		UpdateRotation();
	}

	protected virtual void UpdateRotation()
	{
		rotationTransform.localRotation = directionBehavior.GetHorizontalEntityRotation();
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		directionBehavior = GetComponent<DirectionBehavior>();
	}
}
