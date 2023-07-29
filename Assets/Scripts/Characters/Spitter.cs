using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Spitter : Zombie
{
	[SerializeField] Projectile projectile;
	[SerializeField] Transform shootPos;
	[SerializeField] float shootRange;
	[SerializeField] float shootSpeed;

	[SerializeField] float projectileSpeed;
	// Start is called before the first frame update

	protected override void Awake()
	{
		base.Awake();

	}
	private void Start()
	{
		moveComponent.AttackRange = shootRange;
		moveComponent.Animator.SetFloat("attackSpeed", shootSpeed);
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
		Vector3 dir = (transform.position - moveComponent.Target.transform.position).normalized;

		float distance = Vector3.Distance(transform.position, moveComponent.Target.transform.position);
		projectileSpeed = distance / Time.deltaTime;

		float xVal = Mathf.Clamp(dir.x, -89, 89);
		Quaternion lookQuat = shootPos.rotation;
		lookQuat.x = xVal;

		Quaternion lookRot = Quaternion.LookRotation(-dir);
		shootPos.rotation = lookRot;

		var proj = Instantiate(projectile, shootPos.position, shootPos.rotation);
		proj.ShootProjectile(Vector3.forward * projectileSpeed);

	}



}
/*
 


 */