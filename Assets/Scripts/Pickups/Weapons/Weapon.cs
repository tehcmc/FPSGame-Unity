using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] Camera cam;
	[SerializeField] float bulletRange = 100f;
	[SerializeField] float bulletDamage = 10f;
	[SerializeField] int maxAmmo = 15;

	[SerializeField] Transform muzzle;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] TrailRenderer bulletTrail;

	[SerializeField] ParticleSystem hitParticle;

	public bool CanFire()
	{

		if (!Input.GetButtonDown("Fire1")) return false;

		return true;


	}

	void Update()
	{
		if (CanFire())
		{
			Fire();
		}
	}

	void Fire()
	{

		RaycastHit tr;
		Vector3 hitLoc = cam.transform.forward * bulletRange;

		Debug.DrawLine(cam.transform.position, cam.transform.forward * bulletRange, Color.red);

		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out tr, bulletRange))
		{
			if (tr.transform == transform.parent) return;
			hitLoc = tr.point;

			DoImpactEffect(tr);

			var health = tr.collider.gameObject.GetComponent<Health>();
			if (health)
			{
				health.ModifyHealth(-bulletDamage);

			}

			Debug.Log(tr.collider.name);


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
}

