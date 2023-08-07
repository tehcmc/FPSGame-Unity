using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public enum Firemode
{
	Safe,
	Single,
	Burst,
	Full
}

public enum AttachPointName
{
	Underbarrel,
	TopRail,
	LeftRail,
	RightRail,
	Muzzle,
	Magazine,
	RUTopRail
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

public class RangedWeapon : Weapon
{



	[Header("Values")]
	[SerializeField] protected Firemode firemode = Firemode.Single;

	[Header("Effects")]
	[SerializeField] ParticleSystem defaultMuzzle;
	[SerializeField] protected Transform defaultMuzzlePosition;
	[SerializeField] TrailRenderer bulletTrail;
	[SerializeField] ParticleSystem hitParticle;

	protected bool suppressorAttached = false;

	protected Transform shootPosition;
	ParticleSystem muzzleFlash;


	public int currentAmmo { get; protected set; } = 0;


	public Transform holdPoint;
	protected CameraRecoil recoil;


	protected bool isReloading = false;
	protected bool isShooting = false;


	protected WeaponStats weaponStats;
	public WeaponStats WeaponStats { get => weaponStats; set => weaponStats = value; }


	[Header("Lists")]

	[SerializeField][Tooltip("Set up attach points, drag their transforms into this list")] List<AttachPoint> attachPoints = new();
	[SerializeField][Tooltip("Drag any attachments you wish this weapon to spawn with into this list")] List<Attachment> attachments = new();


	readonly IDictionary<AttachPointName, AttachPoint> attachPointDictionary = new Dictionary<AttachPointName, AttachPoint>();


	public List<Attachment> Attachments { get => attachments; set => attachments = value; }

	IDictionary<AttachPointName, Attachment> attachmentDictionary = new Dictionary<AttachPointName, Attachment>();
	public IDictionary<AttachPointName, Attachment> AttachmentDictionary { get => attachmentDictionary; set => attachmentDictionary = value; }








	public ParticleSystem MuzzleFlash { get => muzzleFlash; set => muzzleFlash = value; }
	public ParticleSystem DefaultMuzzle { get => defaultMuzzle; protected set => defaultMuzzle = value; }


	public Transform ShootPosition { get => shootPosition; set => shootPosition = value; }
	public Transform DefaultMuzzlePosition { get => defaultMuzzlePosition; set => defaultMuzzlePosition = value; }


	public List<AttachPoint> AttachPoints { get => attachPoints; set => attachPoints = value; }
	public bool SuppressorAttached { get => suppressorAttached; set => suppressorAttached = value; }





	void DisableShooting()
	{
		isShooting = true;
	}
	void DoneShooting()
	{
		isShooting = false;
	}




	protected override void Awake()
	{
		base.Awake();
		PopulateAttachPoints();

		weaponStats = GetComponent<WeaponStats>();
		if (attachmentDictionary.ContainsKey(AttachPointName.Muzzle))
		{
			DefaultMuzzlePosition = attachPointDictionary[AttachPointName.Muzzle].PointLocation;
		}

		shootPosition = DefaultMuzzlePosition;
		MuzzleFlash = DefaultMuzzle;


	}


	protected override void Start()
	{
		base.Start();

		recoil = player.GetComponentInChildren<CameraRecoil>();
		foreach (var attachment in Attachments)
		{
			SetupAttachment(attachment);
		}
		if (weaponStats.StatDictionary.ContainsKey(StatType.ClipSize)) currentAmmo = Mathf.RoundToInt(weaponStats.StatDictionary[StatType.ClipSize]);
		GameManager.Instance.ChangeWeapnEvent();

	}
	protected virtual bool CanFire()
	{
		if (isShooting) return false;
		if (currentAmmo <= 0) return false;
		if (isReloading) return false;
		if ((firemode == Firemode.Full && !Input.GetButton("Fire1")) || (firemode == Firemode.Single && !Input.GetButtonDown("Fire1"))) return false;

		return true;
	}

	protected bool CanReload()
	{
		if (isReloading) return false;
		if (!WeaponStats.StatDictionary.ContainsKey(StatType.ClipSize)) return false;
		if (currentAmmo >= (int)WeaponStats.GetStat(StatType.ClipSize)) return false;
		if (isShooting) return false;

		if (player.GetComponent<Ammo>().GetAmmo(weaponType) <= 0) return false;

		if (WeaponStats.GetStat(StatType.ClipSize) <= 0) return false;
		if (!Input.GetKeyDown(KeyCode.R)) return false;
		return true;
	}


	protected virtual void Update()
	{
		if (CanFire())
		{
			Fire();
		}

		if (Input.GetButtonDown("Fire1") && currentAmmo <= 0)
		{
			PlaySound("dryfire");
		}



		if (CanReload()) Reload();

	}


	void FunnyReload()
	{
		if (currentAmmo < WeaponStats.GetStat(StatType.ClipSize))
		{

			currentAmmo++;
			GameManager.Instance.ChangeWeapnEvent();
		}
	}

	protected virtual void Fire()
	{
		isShooting = true;

		if (!suppressorAttached)
		{
			PlaySound("shoot");

		}
		else
		{
			PlaySound("suppressed");
		}


		weaponAnim.SetFloat("speedMult", Mathf.Clamp(weaponStats.GetStat(StatType.FireRate), 0, Mathf.Infinity));
		weaponAnim.SetTrigger("shoot");
		DrawMuzzleEffect();
		ShootBullet();
		currentAmmo--;
		GameManager.Instance.ChangeWeapnEvent();
		Recoil();

	}
	protected void DrawMuzzleEffect()
	{
		var muzzle = ShootPosition;
		var muzzleEffect = Instantiate(MuzzleFlash, muzzle.transform);
		muzzleEffect.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
	}
	protected virtual void ShootBullet()
	{
		RaycastHit tr;


		Vector3 weaponPos = ShootPosition.position + ShootPosition.forward * Mathf.Clamp(weaponStats.GetStat(StatType.Range), 0, Mathf.Infinity);

		Vector3 camPos = Camera.main.transform.position + Camera.main.transform.forward * Mathf.Clamp(weaponStats.GetStat(StatType.Range), 0, Mathf.Infinity);

		Vector3 shootDirection = SpreadBullet();

		Vector3 hitLoc = ShootPosition.position + shootDirection * Mathf.Clamp(weaponStats.GetStat(StatType.Range), 0, Mathf.Infinity);

		int layerMask = 1 << 31;
		layerMask = ~layerMask;// invert layermask. Only layer 31 will be ignored.


		if (Physics.Raycast(Camera.main.transform.position, shootDirection, out tr, Mathf.Clamp(weaponStats.GetStat(StatType.Range), layerMask, Mathf.Infinity), layerMask, QueryTriggerInteraction.Ignore))
		{
			if (tr.transform == transform.parent) return;
			if (tr.transform == null) return;

			hitLoc = tr.point;
			if (tr.transform == null) return;
			DoImpactEffect(tr);
			DamageCharacter(tr);
		}


		ShootTrail(hitLoc);
	}
	protected override void OnDisable()
	{
		isReloading = false;
		isShooting = false;
		base.OnDisable();
	}
	private Vector3 SpreadBullet()
	{
		Vector3 shootDirection = ShootPosition.forward;


		shootDirection.x += Random.Range(-weaponStats.GetStat(StatType.HorizontalSpread), weaponStats.GetStat(StatType.HorizontalSpread));

		shootDirection.y += Random.Range(-weaponStats.GetStat(StatType.VerticalSpread), weaponStats.GetStat(StatType.VerticalSpread));

		shootDirection.z += Random.Range(-weaponStats.GetStat(StatType.VerticalSpread), weaponStats.GetStat(StatType.VerticalSpread));

		shootDirection.Normalize();

		return shootDirection;
	}

	protected void Recoil()
	{
		float xRecoil = Mathf.Clamp(-weaponStats.GetStat(StatType.VerticalRecoil), -Mathf.Infinity, 0);
		float yRecoil = Mathf.Clamp(-weaponStats.GetStat(StatType.HorizontalRecoil), -Mathf.Infinity, 0);
		float zRecoil = Mathf.Clamp(-weaponStats.GetStat(StatType.VerticalRecoil), -Mathf.Infinity, 0);
		recoil.RecoilFire(xRecoil, yRecoil, zRecoil);
	}

	protected void DamageCharacter(RaycastHit tr)
	{

		string hitPoint = "";


		var character = tr.collider.GetComponentInParent<Character>();
		Debug.Log(character);
		if (!character) return;

		var colliderName = tr.collider.GetComponent<NamedCollider>();
		if (colliderName) hitPoint = colliderName.ColliderName;
		Debug.Log(colliderName.ColliderName);


		var health = character.GetComponent<Health>();
		float mult = 1;
		mult = character.GetDamageMultiplier(hitPoint);

		DamagePoint damagePoint = character.GetDamagePoint(hitPoint);

		if (damagePoint != null && (colliderName.ColliderName.ToUpper() == "HEAD"))
		{
			player.AudioSource.PlayOneShot(GameManager.Instance.HeadshotSound);

		}


		float damage = (weaponStats.GetStat(StatType.BaseDamage) * weaponStats.GetStat(StatType.DamageMultiplier)) * mult;

		health.DamageHealth(damage);
		ShowDamage(damage, tr.point);

	}

	protected void ShootTrail(Vector3 hitLoc)
	{
		var muzzle = ShootPosition;
		TrailRenderer trail = Instantiate(bulletTrail, muzzle.position, Quaternion.identity);
		StartCoroutine(SpawnTrail(trail, hitLoc));
	}



	protected void DoImpactEffect(RaycastHit tr)
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


	protected virtual void Reload()
	{

		isReloading = true;
		weaponAnim.SetTrigger("reload");
		weaponAnim.SetFloat("reloadSpeed", WeaponStats.GetStat(StatType.ReloadSpeed));

	}

	protected virtual void ReloadFinished()
	{
		var ammoComp = player.GetComponent<Ammo>();
		if (!ammoComp) return;


		var clip = Mathf.RoundToInt(weaponStats.GetStat(StatType.ClipSize));

		var amount = ammoComp.TakeAmmo(weaponType, clip - currentAmmo);

		Debug.Log(ammoComp.GetAmmo(weaponType));
		currentAmmo += amount;
		Debug.Log($"Current ammo (Reload): {currentAmmo}");
		GameManager.Instance.ChangeWeapnEvent();
		isReloading = false;
	}


	void PopulateAttachPoints() //populate attachment point dictionary with attachment point list entries
	{
		foreach (var point in AttachPoints)
		{
			if (!attachPointDictionary.ContainsKey(point.AttachmentPoint))
			{
				attachPointDictionary.Add(point.AttachmentPoint, point);
			}
		}
	}






	public void SetupAttachment(Attachment attachment)
	{
		if (!attachPointDictionary.ContainsKey(attachment.MyAttachPoint)) return;

		if (AttachmentDictionary.ContainsKey(attachment.MyAttachPoint)) return;

		AttachPoint point = attachPointDictionary[attachment.MyAttachPoint];

		if (point.IsOccupied) return;

		if (!attachment.CheckIfValid(weaponType)) return; // if attachment does not accept this weapon type, return.


		attachment = Instantiate(attachment, transform);
		AttachmentDictionary.Add(attachment.MyAttachPoint, attachment);

		attachment.transform.parent = attachPointDictionary[attachment.MyAttachPoint].PointLocation;
		attachment.transform.position = attachment.transform.parent.position;
		attachPointDictionary[attachment.MyAttachPoint].IsOccupied = true;
		GameManager.Instance.ChangeWeapnEvent();
	}


	Transform GetAttachPoint(AttachPointName pointName)
	{
		if (!attachPointDictionary.ContainsKey(pointName)) return null;

		return attachPointDictionary[pointName].PointLocation;

	}

	public void RemoveAttachment(AttachPointName point)
	{
		if (!AttachmentDictionary.ContainsKey(point)) return;

		AttachmentDictionary[point].gameObject.SetActive(false);
		Destroy(AttachmentDictionary[point].gameObject);
		attachPointDictionary[point].IsOccupied = false;
		AttachmentDictionary.Remove(point);
		GameManager.Instance.ChangeWeapnEvent();
	}



	void RemoveAllAttachments()
	{

		foreach (var attachment in Attachments)
		{
			attachment.gameObject.SetActive(false);
			Destroy(attachment);
		}

	}

}

