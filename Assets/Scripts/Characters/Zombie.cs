using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCMover))]
public class Zombie : Character
{

	/// <summary>
	///  add new zombie types? 
	///  
	/// HARD
	///  charger - detect player from afar, run really fast in straight line to player (if path is possible) if player is collided with act like l4d charger
	///  
	/// 
	/// MEDIUM
	///  spitter - spit projectile at player
	///  tank - lots of hp lots of damage, knockback on attacks
	///  
	///  EASY
	///  stalker - very fast, lower hp
	///  regular - normal hp, lots of them.
	///  
	/// </summary>

	protected NPCMover moveComponent;

	[SerializeField] float attackDamage = 10f;

	[SerializeField] int pointValue = 10;
	protected override void Awake()
	{
		base.Awake();
		moveComponent = GetComponent<NPCMover>();
	}

	// Update is called once per frame
	void Update()
	{

	}
	protected override void TakeDamage()
	{
		base.TakeDamage();

	}


	public virtual void DoAttack()
	{
		var target = moveComponent.Target;
		if (!target) return;

		var health = target.GetComponent<Health>();
		if (health)
		{
			health.DamageHealth(-attackDamage);
		}
		else
		{
			Debug.LogError("No Health component on target!");
		}
	}

	protected override void Die()
	{
		base.Die();
		var player = FindObjectOfType<Player>();
		player.GetComponent<PointBank>().AddPoints(pointValue);
		Destroy(gameObject);
	}
}
