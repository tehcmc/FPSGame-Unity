using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpit : Projectile
{
	[SerializeField] float projectileDamage = 10;
	bool hit = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	protected override void OnCollisionEnter(Collision collision)
	{


	}

	private void OnTriggerEnter(Collider other)
	{
		if (!hit)
		{
			hit = true;
			DamageCharacter(other);
			Destroy(gameObject);
		}

	}
	protected void DamageCharacter(Collider collision)
	{
		if (collision == null) return;
		var character = collision.gameObject.transform.parent.GetComponentInChildren<Character>();
		if (!character) return;

		var health = character.GetComponent<Health>();
		float mult = character.GetDamageMultiplier(collision.transform);

		DamagePoint damagePoint = character.GetDamagePoint(collision.transform);


		float damage = projectileDamage * mult;

		health.DamageHealth(damage);
		ShowDamage(damage, transform.position);

	}
}
