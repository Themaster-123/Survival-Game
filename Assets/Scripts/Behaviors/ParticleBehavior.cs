using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior : Behavior
{
	public GameObject bloodParticle;
	protected HealthBehavior healthBehavior;

	protected void Start()
	{
		healthBehavior.OnDamageEvent += OnDamage;
	}

	protected virtual void OnDamage(HealthBehavior healthBehavior)
	{
		Instantiate(bloodParticle, transform.position, Quaternion.identity);
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		healthBehavior = GetComponent<HealthBehavior>();
	}
}
