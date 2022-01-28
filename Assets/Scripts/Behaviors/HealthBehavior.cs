using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : Behavior
{
	public event Action<HealthBehavior> OnDeathEvent;

	public event Action<HealthBehavior> OnDamageEvent;

	public virtual float MaxHealth
	{
		get
		{
			return maxHealth;
		}

		set
		{
			float oldMaxHealth = maxHealth;
			maxHealth = Mathf.Max(0, value);
			Health = Health == oldMaxHealth ? maxHealth : Mathf.Min(Health, maxHealth);
		}
	}

	public virtual float Health
	{
		get
		{
			return health;
		}

		set
		{
			health = Mathf.Clamp(value, 0, maxHealth);

			if (health == 0)
			{
				Kill();
			}
		}
	}

	[SerializeField]

	protected float maxHealth = 100;

	[SerializeField]
	protected float health = 100;

	// triggers OnDeathEvent for DeathBehavior's to use
	public virtual void Kill()
	{
		OnDeathEvent?.Invoke(this);
	}

	public virtual void Damage(float amount)
	{
		Health -= amount;
		OnDamageEvent?.Invoke(this);
	}

	protected virtual void OnValidate()
	{
		health = Mathf.Clamp(health, 0, maxHealth);
	}
}
