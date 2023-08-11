using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	//explosion - trigger, then expand sphere until it reaches explosion radius. 
	SphereCollider sph;
	[SerializeField] float explosionRadius = 5f;
	[SerializeField] float expansionRate = 0.01f;
	[SerializeField] bool triggered = false;
	[SerializeField][Tooltip("Will this explosion instantly reach its explosion radius, or will it expand to it over time")] bool instantExplosion = false;
	[SerializeField] float knockback = 1f;
	[SerializeField] float explosionDamage = 100f;
	[SerializeField] float explosionLifetime = 0.1f;

	[SerializeField] ParticleSystem explosionParticle;

	float timePassed = 0;

	public bool InstantExplosion { get => instantExplosion; set => instantExplosion = value; }
	public float ExplosionRadius { get => explosionRadius; set => explosionRadius = value; }
	public float ExpansionRate { get => expansionRate; set => expansionRate = value; }
	public float Knockback { get => knockback; set => knockback = value; }
	public float ExplosionDamage { get => explosionDamage; set => explosionDamage = value; }
	public float ExplosionLifetime { get => explosionLifetime; set => explosionLifetime = value; }

	private void Awake()
	{
		sph = gameObject.AddComponent<SphereCollider>();
		gameObject.layer = 30;
		sph.isTrigger = true;
		sph.radius = 0;
	}

	private void Update()
	{
		if (triggered)
		{
			timePassed += Time.deltaTime;
			if (sph.radius < ExplosionRadius)
			{
				sph.radius += ExpansionRate * Time.deltaTime;
			}


			if (timePassed > ExplosionLifetime && sph.radius >= ExplosionRadius)
			{
				ExplosionFinished();
			}
		}

	}

	public void Explode() // explode using default prefab variables
	{
		triggered = true;

		if (InstantExplosion)
		{

			sph.radius = ExplosionRadius;
			ExplosionLifetime = .5f;
		}
		Instantiate(explosionParticle, transform.position, Quaternion.identity);
	}

	public void Explode(float radius, float damage, bool instant) // instant explosion
	{
		explosionRadius = radius;
		explosionDamage = damage;
		instantExplosion = instant;
		Explode();
	}
	public void Explode(float radius, float damage, float lifetime, float expansion) // explosion that expands over defined time
	{
		explosionRadius = radius;
		expansionRate = expansion;
		explosionDamage = damage;
		explosionLifetime = lifetime;
		Explode();
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Enter: " + other.gameObject.name);
		DamageCharacter(other);
	}

	void DamageCharacter(Collider other)
	{
		var character = other.GetComponentInParent<Character>();
		if (!character) return;
		var healh = character.GetComponent<Health>();
		if (!healh) return;
		healh.DamageHealth(explosionDamage);
	}

	void ExplosionFinished()
	{
		sph.enabled = false;
		Destroy(gameObject);
	}
}
