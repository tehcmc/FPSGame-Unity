using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class Grenade : Projectile
{
	[SerializeField][Tooltip("If true, this grenade will explode on impact with a surface")] bool impactGrenade = false;
	[SerializeField] float grenadeTimer = 3f;
	[SerializeField] Explosion explosion;
	[SerializeField] float radius = 5f;
	[SerializeField] float damage = 100f;
	bool impacted = false;

	void Start()
	{
		if (!explosion) Destroy(gameObject);
	}


	void Update()
	{
		currentTime += Time.deltaTime;
		if (CanExplode())
		{
			ExplodeGrenade();
		}
	}

	bool CanExplode()
	{
		if (currentTime < grenadeTimer) return false;

		return true;
	}


	void ExplodeGrenade()
	{
		var explosionObject = Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
		explosionObject.GetComponent<Explosion>().Explode(radius, damage, true);
		Destroy(gameObject);
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		if (!impactGrenade) return;

		ExplodeGrenade();
	}
}
