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
		string hitPoint = "";
		var character = collision.gameObject.transform.GetComponentInParent<Character>();
		if (!character) return;
		var colliderName = collision.GetComponent<NamedCollider>();
		if (colliderName) hitPoint = colliderName.ColliderName;
		Debug.Log(hitPoint);
		var health = character.GetComponent<Health>();
		float mult = character.GetDamageMultiplier(hitPoint);

		DamagePoint damagePoint = character.GetDamagePoint(hitPoint);


		float damage = projectileDamage * mult;

		health.DamageHealth(damage);
		ShowDamage(damage, transform.position);

	}
}
