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
			RaycastHit hit = interactableBehavior.Raycast(~0);
			MonoBehaviour.print(hit.collider);
			if (hit.collider == null) return;
			HealthBehavior entityHealthBehavior = hit.transform.gameObject.GetComponent<HealthBehavior>();
			if (entityHealthBehavior != null)
			{
				entityHealthBehavior.Damage(damage);
			}
		}
	}
}
