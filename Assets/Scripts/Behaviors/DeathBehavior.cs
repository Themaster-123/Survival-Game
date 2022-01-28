using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBehavior))]
public class DeathBehavior : Behavior
{
    protected HealthBehavior healthBehavior;

	protected override void GetComponents()
	{
		base.GetComponents();
		healthBehavior = GetComponent<HealthBehavior>();
		healthBehavior.OnDeathEvent += OnDeath;
	}

	protected virtual void OnDeath(HealthBehavior healthBehavior)
	{
		Destroy(healthBehavior.gameObject);
	}
}
