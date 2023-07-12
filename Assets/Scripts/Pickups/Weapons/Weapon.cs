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

	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] TrailRenderer bulletTrail;

	[SerializeField] ParticleSystem hitParticle;

	[SerializeField] protected List<Attachment> attachments = new();

	[SerializeField] protected List<AttachPoint> attachPoints = new();

	IDictionary<AttachPointName, AttachPoint> attachPointDictionary = new Dictionary<AttachPointName, AttachPoint>();

	IDictionary<AttachPointName, Attachment> attachmentDictionary = new Dictionary<AttachPointName, Attachment>();


	WeaponStats weaponStats;

	public WeaponStats WeaponStats { get => weaponStats; set => weaponStats = value; }


	[SerializeField] Firemode firemode = Firemode.Single;




	float fireTime = 0;

	private void Awake()
	{
		weaponStats = GetComponent<WeaponStats>();
		PopulateAttachPoints();
	}
	private void Start()
	{
		var player = FindObjectOfType<Player>();
		if (!player) Destroy(gameObject);

		cam = player.GetComponentInChildren<Camera>();

		foreach (var attachment in attachments)
		{
			SetupAttachment(attachment);
		}

	}
	protected virtual bool CanFire()
	{
		if (fireTime < Mathf.Clamp(weaponStats.GetStat(StatType.FireRate), 0, Mathf.Infinity)) return false;

		if (firemode == Firemode.Full && !Input.GetButton("Fire1")) return false;

		if (firemode == Firemode.Single && !Input.GetButtonDown("Fire1")) return false;

		return true;
	}

	void Update()
	{
		if (fireTime < Mathf.Clamp(weaponStats.GetStat(StatType.FireRate), 0, Mathf.Infinity))
		{
			fireTime += Time.deltaTime;
		}

		if (CanFire())
		{
			Fire();
		}


		if (Input.GetKeyDown(KeyCode.O))
		{
			RemoveAttachment(AttachPointName.Muzzle);
		}


	}
	void Fire()
	{
		fireTime = 0;
		RaycastHit tr;
		Vector3 hitLoc = cam.transform.position + cam.transform.forward * bulletRange;

		//Debug.DrawLine(cam.transform.position, cam.transform.forward * bulletRange, Color.red);

		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out tr, Mathf.Clamp(weaponStats.GetStat(StatType.Range), 0, Mathf.Infinity), 99, QueryTriggerInteraction.Ignore))
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

		DoFireEffects(hitLoc);

	}

	private void DoFireEffects(Vector3 hitLoc)
	{
		var muzzle = GetAttachPoint(AttachPointName.Muzzle);
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
		if (!attachPointDictionary.ContainsKey(attachment.MyAttachPoint)) return;

		if (attachmentDictionary.ContainsKey(attachment.MyAttachPoint)) return;


		AttachPoint point = attachPointDictionary[attachment.MyAttachPoint];
		if (point.IsOccupied) return;

		attachment = Instantiate(attachment, transform);
		attachmentDictionary.Add(attachment.MyAttachPoint, attachment);

		attachment.transform.parent = attachPointDictionary[attachment.MyAttachPoint].PointLocation;
		attachment.transform.position = attachment.transform.parent.position;
		attachPointDictionary[attachment.MyAttachPoint].IsOccupied = true;

	}






	void PopulateAttachPoints()
	{
		foreach (var point in attachPoints)
		{
			if (!attachPointDictionary.ContainsKey(point.AttachmentPoint))
			{
				attachPointDictionary.Add(point.AttachmentPoint, point);
			}

		}
	}

	Transform GetAttachPoint(AttachPointName pointName)
	{
		if (!attachPointDictionary.ContainsKey(pointName)) return null;

		return attachPointDictionary[pointName].PointLocation;

	}

	public void RemoveAttachment(AttachPointName point)
	{
		if (!attachmentDictionary.ContainsKey(point)) return;

		attachmentDictionary[point].gameObject.SetActive(false);
		Destroy(attachmentDictionary[point].gameObject);
		attachPointDictionary[point].IsOccupied = false;
		attachmentDictionary.Remove(point);
	}

	void RemoveAllAttachments()
	{
		foreach (var attachment in attachments)
		{
			attachment.gameObject.SetActive(false);
			Destroy(attachment);
		}
	}
}

