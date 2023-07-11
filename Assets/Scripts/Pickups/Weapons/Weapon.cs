using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public enum Firemode
{
	Safe,
	Single,
	Burst,
	Full
}

public enum WeaponType
{
	Pistol,
	SMG,
	Rifle,
	Sniper,
	Shotgun,
	Special
}

public enum AttachPointName
{
	Underbarrel,
	TopRail,
	LeftRail,
	RightRail,
	Muzzle,
	Magazine

}

[Serializable]
public class AttachPoint
{
	[SerializeField] AttachPointName attachmentPoint;
	[SerializeField] Transform pointLocation;
	bool isOccupied = false;

	public AttachPointName AttachmentPoint { get => attachmentPoint; set => attachmentPoint = value; }
	public Transform PointLocation { get => pointLocation; set => pointLocation = value; }
	public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
}



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

	[SerializeField] protected List<AttachPoint> attachPoints = new();

	IDictionary<AttachPointName, AttachPoint> attachmentPointDictionary = new Dictionary<AttachPointName, AttachPoint>();


	WeaponStats weaponStats;

	internal WeaponStats WeaponStats { get => weaponStats; set => weaponStats = value; }


	[SerializeField] Firemode firemode = Firemode.Single;




	float fireTime = 0;

	private void Awake()
	{
		WeaponStats = GetComponent<WeaponStats>();
		PopulateAttachPoints();
	}
	protected virtual bool CanFire()
	{
		if (fireTime < weaponStats.GetStat(StatType.FireRate)) return false;

		if (firemode == Firemode.Full && !Input.GetButton("Fire1")) return false;

		if (firemode == Firemode.Single && !Input.GetButtonDown("Fire1")) return false;

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

		//Debug.DrawLine(cam.transform.position, cam.transform.forward * bulletRange, Color.red);

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
		if (!attachmentPointDictionary.ContainsKey(attachment.MyAttachPoint)) return;
		AttachPoint point = attachmentPointDictionary[attachment.MyAttachPoint];
		if (point.IsOccupied) return;

		attachment = Instantiate(attachment, transform);
		attachments.Add(attachment);

		attachment.transform.parent = attachmentPointDictionary[attachment.MyAttachPoint].PointLocation;
		attachment.transform.position = attachment.transform.parent.position;
		attachmentPointDictionary[attachment.MyAttachPoint].IsOccupied = true;


	}


	void PopulateAttachPoints()
	{
		foreach (var point in attachPoints)
		{
			if (!attachmentPointDictionary.ContainsKey(point.AttachmentPoint))
			{
				attachmentPointDictionary.Add(point.AttachmentPoint, point);
			}

		}
	}
}

