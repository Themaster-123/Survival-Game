using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : Behavior
{
	public float maxHealth = 100;
	public float health = 100;

	protected void OnValidate()
	{
		health = Mathf.Clamp(health, 0, maxHealth);
	}
}
