using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCMover))]
public class Zombie : Character
{
	NPCMover moveComponent;

	[SerializeField] float attackDamage = 10f;
	protected override void Awake()
	{
		base.Awake();
		moveComponent = GetComponent<NPCMover>();
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	protected override void TakeDamage()
	{
		base.TakeDamage();
	}


	public void DoAttack()
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
		Destroy(gameObject);
	}
}
