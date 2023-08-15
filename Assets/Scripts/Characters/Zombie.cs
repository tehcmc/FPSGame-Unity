using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCMover))]
[RequireComponent(typeof(Animator))]
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
	protected Animator animator;

	[SerializeField] float attackDamage = 10f;

	[SerializeField] int pointValue = 10;

	[SerializeField] float attackSpeed = 1f;
	protected override void Awake()
	{
		base.Awake();
		moveComponent = GetComponent<NPCMover>();
		animator = GetComponent<Animator>();
		animator.SetFloat("attackSpeed", attackSpeed);
	}

	// Update is called once per frame
	void Update()
	{

	}
	protected override void TakeDamage()
	{
		base.TakeDamage();
		animator.SetTrigger("onHit");
	}


	public virtual void DoAttack()
	{
		var target = moveComponent.Target;
		if (!target) return;
		Debug.Log("attk");
		var health = target.GetComponent<Health>();
		if (health)
		{
			Debug.Log("dmg");
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
		moveComponent.CurrentState = State.Die;
		animator.SetTrigger("dead");
		enabled = false;
	}
}
