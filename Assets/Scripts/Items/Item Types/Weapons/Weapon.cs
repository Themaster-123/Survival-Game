using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
	public float damage;
	public Weapon(ItemSettings itemSettings, float damage) : base(itemSettings)
	{
		this.damage = damage;
	}

	public override void Use(ItemBehavior itemBehavior, GameObject caller)
	{
		InteractableBehavior interactableBehavior = caller.GetComponent<InteractableBehavior>();
		if (interactableBehavior != null)
		{
			AnimationUtils.StopAnimation(itemBehavior.transform);
			AnimationUtils.PingPongRotate(itemBehavior.transform, Vector3.right, 4f, 45);
			AnimationUtils.PingPongMove(itemBehavior.transform, Vector3.forward, .5f, 4f);
			RaycastHit hit = interactableBehavior.Raycast(~0);
			if (hit.collider == null) return;
			HealthBehavior entityHealthBehavior = hit.transform.gameObject.GetComponent<HealthBehavior>();
			if (entityHealthBehavior != null)
			{
				entityHealthBehavior.Damage(damage);
			}
		}
	}
}
