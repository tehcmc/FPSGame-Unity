using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatType
{
	VerticalRecoil,
	HorizontalRecoil,
	FireRate,
	BaseDamage,
	DamageMultiplier,
	Range
}


[RequireComponent(typeof(WeaponStats))]
public class Weapon : MonoBehaviour
{
	[SerializeField] List<Stat> stats = new();

	IDictionary<StatType, float> statDictionary = new Dictionary<StatType, float>();


	[SerializeField] Camera cam;
	[SerializeField] float bulletRange = 100f;
	[SerializeField] int maxAmmo = 15;

	[SerializeField] Transform muzzle;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] TrailRenderer bulletTrail;

	[SerializeField] ParticleSystem hitParticle;

	[SerializeField] protected List<Attachment> attachments = new();

	WeaponStats weaponStats;

	internal WeaponStats WeaponStats { get => weaponStats; set => weaponStats = value; }

	public List<Stat> Stats { get => stats; set => stats = value; }

	float fireTime = 0;

	private void Awake()
	{
		WeaponStats = GetComponent<WeaponStats>();
		foreach (var stat in stats)
		{
			statDictionary.Add(stat.StatType, stat.Value);
		}

	}
	public bool CanFire()
	{
		if (fireTime < weaponStats.FireRate) return false;

		if (!Input.GetButton("Fire1")) return false;

		return true;
	}

	void Update()
	{
		if (fireTime < weaponStats.FireRate)
		{
			fireTime += Time.deltaTime;
		}

		if (CanFire())
		{
			Fire();
		}

	}
	void Fire()
	{
		fireTime = 0;
		RaycastHit tr;
		Vector3 hitLoc = cam.transform.forward * bulletRange;

		Debug.DrawLine(cam.transform.position, cam.transform.forward * bulletRange, Color.red);

		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out tr, bulletRange, 99, QueryTriggerInteraction.Ignore))
		{
			if (tr.transform == transform.parent) return;
			hitLoc = tr.point;

			DoImpactEffect(tr);
			var health = tr.collider.gameObject.GetComponent<Health>();
			if (health)
			{
				health.DamageHealth(weaponStats.BaseDamage * weaponStats.DamageMultiplier);

			}

			//	Debug.Log(tr.collider.name);


		}
		TrailRenderer trail = Instantiate(bulletTrail, muzzle.position, Quaternion.identity);
		var muzzleEffect = Instantiate(muzzleFlash, muzzle.transform);
		muzzleEffect.transform.position = muzzle.position; muzzleEffect.transform.rotation = muzzle.rotation;
		StartCoroutine(SpawnTrail(trail, hitLoc));

	}
	void DoImpactEffect(RaycastHit tr)
	{
		Instantiate(hitParticle, tr.point, Quaternion.LookRotation(tr.normal));
	}


	IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitLoc)
	{

		float time = 0;
		Vector3 startPos = trail.transform.position;
		while (time < 1)
		{
			trail.transform.position = Vector3.Lerp(startPos, hitLoc, time);
			time += Time.deltaTime / trail.time;
			yield return null;
		}
		trail.transform.position = hitLoc;
		Destroy(trail.gameObject);

	}

	void SetStats()
	{
		int statRange = Enum.GetNames(typeof(Stat)).Length;
		for (int i = 0; i < statRange; i++)
		{
			StatType currentStat = (StatType)i;

			Stats.Add(new Stat(currentStat, 0));
		}
	}

	public void SetupAttachment(Attachment attachment)
	{
		attachment = Instantiate(attachment, transform);

		attachments.Add(attachment);

	}
}

