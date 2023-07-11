using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[RequireComponent(typeof(WeaponStats))]
public class Weapon : MonoBehaviour
{




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





	float fireTime = 0;

	private void Awake()
	{
		WeaponStats = GetComponent<WeaponStats>();

	}
	public bool CanFire()
	{
		if (fireTime < weaponStats.GetStat(StatType.FireRate)) return false;

		if (!Input.GetButton("Fire1")) return false;

		return true;
	}

	void Update()
	{
		if (fireTime < weaponStats.GetStat(StatType.FireRate))
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
				health.DamageHealth(weaponStats.GetStat(StatType.BaseDamage) * weaponStats.GetStat(StatType.DamageMultiplier));

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


	public void SetupAttachment(Attachment attachment)
	{
		attachment = Instantiate(attachment, transform);

		attachments.Add(attachment);

	}
}

