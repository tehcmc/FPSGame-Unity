using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : Zombie
{
	[SerializeField] Projectile projectile;
	[SerializeField] Transform shootPos;
	[SerializeField] float shootRange;
	// Start is called before the first frame update

	protected override void Awake()
	{
		base.Awake();
		moveComponent.AttackRange = shootRange;
	}



	// Update is called once per frame
	void Update()
	{

	}

	public override void DoAttack()
	{
		SpitProjectile();
	}

	void SpitProjectile()
	{
		var proj = Instantiate(projectile, shootPos.position, transform.rotation);
	}
}
