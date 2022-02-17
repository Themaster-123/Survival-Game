using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(InteractableBehavior))]
[RequireComponent(typeof(DirectionBehavior))]
public class BuildingBehavior : Behavior
{
	protected Entity entity;
	protected InteractableBehavior interactableBehavior;
	protected DirectionBehavior directionBehavior;

	public void Place(Building building)
	{
		RaycastHit hit = interactableBehavior.Raycast(~0);
		entity.world.PlaceBuilding(hit.point + hit.normal * .1f, directionBehavior.GetHorizontalEntityRotation(), building);
	}

	public void RemoveBuilding()
	{
		RaycastHit hit = interactableBehavior.Raycast(~0);
		
		if (hit.collider != null && hit.collider.TryGetComponent(out Building building))
		{
			entity.world.RemoveBuilding(building);
		}
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		entity = GetComponent<Entity>();
		interactableBehavior = GetComponent<InteractableBehavior>();
		directionBehavior = GetComponent<DirectionBehavior>();
	}
}
