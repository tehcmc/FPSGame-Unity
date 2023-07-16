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

public enum WeaponType
{
	Pistol,
	SMG,
	Rifle,
	Shotgun,
	Sniper,
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
public class RangedWeapon : MonoBehaviour
{




	[Header("Values")]
	[SerializeField] string weaponName;
	[SerializeField] WeaponType weaponType;
	[SerializeField] Firemode firemode = Firemode.Single;

	[Header("Effects")]
	[SerializeField] ParticleSystem defaultMuzzle;
	[SerializeField]
	AudioClip defaultFireSound
	;
	[SerializeField] AudioClip triggerSound;


	[SerializeField] TrailRenderer bulletTrail;
	[SerializeField] ParticleSystem hitParticle;


	[Header("Lists")]
	[SerializeField][Tooltip("Drag any attachments you wish this weapon to spawn with into this list")] List<Attachment> attachments = new();
	[SerializeField][Tooltip("Set up attach points, drag their transforms into this list")] List<AttachPoint> attachPoints = new();


	Transform shootPosition;
	Transform defaultMuzzlePosition;

	[SerializeField] Animator weaponAnim;


	IDictionary<AttachPointName, AttachPoint> attachPointDictionary = new Dictionary<AttachPointName, AttachPoint>();

	IDictionary<AttachPointName, Attachment> attachmentDictionary = new Dictionary<AttachPointName, Attachment>();


	WeaponStats weaponStats;
	Camera cam;
	Player player;
	AudioSource audioSource;

	ParticleSystem muzzleFlash;
	AudioClip fireSound;

	float defaultAnimSpeed;

	public int currentAmmo { get; protected set; } = 0;
	protected float fireTime = 0;

	public GameObject weaponModel;
	public Transform holdPoint;

	public WeaponStats WeaponStats { get => weaponStats; set => weaponStats = value; }
	public string WeaponName { get => weaponName; set => weaponName = value; }
	public List<Attachment> Attachments { get => attachments; set => attachments = value; }
	public IDictionary<AttachPointName, Attachment> AttachmentDictionary { get => attachmentDictionary; set => attachmentDictionary = value; }
	public WeaponType WeaponType { get => weaponType; set => weaponType = value; }





	public ParticleSystem MuzzleFlash { get => muzzleFlash; set => muzzleFlash = value; }
	public AudioClip FireSound { get => fireSound; set => fireSound = value; }
	public AudioClip DefaultFireSound { get => defaultFireSound; protected set => defaultFireSound = value; }
	public ParticleSystem DefaultMuzzle { get => defaultMuzzle; protected set => defaultMuzzle = value; }
	public Transform ShootPosition { get => shootPosition; set => shootPosition = value; }
	public Transform DefaultMuzzlePosition { get => defaultMuzzlePosition; set => defaultMuzzlePosition = value; }



	bool isReloading = false;







	protected virtual void Awake()
	{
		PopulateAttachPoints();
		weaponStats = GetComponent<WeaponStats>();

		audioSource = GetComponent<AudioSource>();
		DefaultMuzzlePosition = attachPointDictionary[AttachPointName.Muzzle].PointLocation;
		shootPosition = DefaultMuzzlePosition;
		MuzzleFlash = DefaultMuzzle;
		FireSound = DefaultFireSound;
		weaponAnim = GetComponentInChildren<Animator>();
	}

	private void OnEnable()
	{
		Debug.Log("Wep enable");
		GameManager.Instance.ChangeWeapnEvent();

	}
	private void OnDisable()
	{
		GameManager.Instance.ChangeWeapnEvent();
	}
	protected virtual void Start()
	{
		if (weaponAnim) defaultAnimSpeed = weaponAnim.speed;

		player = FindObjectOfType<Player>();
		if (!player) Destroy(gameObject);

		cam = player.GetComponentInChildren<Camera>();


		foreach (var attachment in Attachments)
		{
			SetupAttachment(attachment);
		}

		if (weaponStats.StatDictionary.ContainsKey(StatType.ClipSize)) currentAmmo = Mathf.RoundToInt(weaponStats.StatDictionary[StatType.ClipSize]);
		GameManager.Instance.ChangeWeapnEvent();

	}
	protected virtual bool CanFire()
	{
		if (currentAmmo <= 0) return false;
		if (isReloading) return false;

		if (fireTime < Mathf.Clamp(weaponStats.GetStat(StatType.FireRate), 0, Mathf.Infinity)) return false;

		if ((firemode == Firemode.Full && !Input.GetButton("Fire1")) || (firemode == Firemode.Single && !Input.GetButtonDown("Fire1"))) return false;

		return true;
	}

	protected bool CanReload()
	{
		if (weaponAnim && isReloading) return false;
		if (!WeaponStats.StatDictionary.ContainsKey(StatType.ClipSize)) return false;
		if (currentAmmo >= (int)WeaponStats.GetStat(StatType.ClipSize)) return false;
		if (WeaponStats.GetStat(StatType.ClipSize) <= 0) return false;

		return true;
	}


	protected virtual void Update()
	{
		if (fireTime < Mathf.Clamp(weaponStats.GetStat(StatType.FireRate), 0, Mathf.Infinity))
		{
			fireTime += Time.deltaTime;
		}



		if (CanFire())
		{
			if (weaponAnim) weaponAnim.speed = Mathf.Clamp(3, 1, Mathf.Infinity);
			PlaySound(FireSound);

			if (weaponAnim) weaponAnim.SetTrigger("shoot");

			Fire();
			fireTime = 0;
			currentAmmo--;
			GameManager.Instance.ChangeWeapnEvent();
			if (weaponAnim) weaponAnim.speed = defaultAnimSpeed;
		}

		if (Input.GetButtonDown("Fire1") && currentAmmo <= 0)
		{
			PlaySound(triggerSound);
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (!CanReload()) return;
			Reload();
		}


	}


	protected virtual void Fire()
	{

		RaycastHit tr;

		Vector3 hitLoc = ShootPosition.position + ShootPosition.forward * Mathf.Clamp(weaponStats.GetStat(StatType.Range), 0, Mathf.Infinity);


		Vector3 shootDirection = ShootPosition.forward;


		shootDirection.x += Random.Range(-weaponStats.GetStat(StatType.HorizontalSpread), weaponStats.GetStat(StatType.HorizontalSpread));

		shootDirection.y += Random.Range(-weaponStats.GetStat(StatType.VerticalSpread), weaponStats.GetStat(StatType.VerticalSpread));

		shootDirection.z += Random.Range(-weaponStats.GetStat(StatType.VerticalSpread), weaponStats.GetStat(StatType.VerticalSpread));

		shootDirection.Normalize();


		hitLoc = ShootPosition.position + shootDirection * Mathf.Clamp(weaponStats.GetStat(StatType.Range), 0, Mathf.Infinity);


		//Debug.DrawLine(cam.transform.position, cam.transform.forward * bulletRange, Color.red);
		Debug.Log("bang");
		if (Physics.Raycast(ShootPosition.position, shootDirection, out tr, Mathf.Clamp(weaponStats.GetStat(StatType.Range), 0, Mathf.Infinity), 99, QueryTriggerInteraction.Ignore))
		{
			if (tr.transform == transform.parent) return;
			hitLoc = tr.point;

			DoImpactEffect(tr);
			var health = tr.collider.gameObject.GetComponent<Health>();
			if (health)
			{
				float damage = weaponStats.GetStat(StatType.BaseDamage) * weaponStats.GetStat(StatType.DamageMultiplier);
				health.DamageHealth(damage);
				ShowDamage(damage, tr.point);
			}
		}

		DoFireEffects(hitLoc);

	}

	private void DoFireEffects(Vector3 hitLoc)
	{
		var muzzle = GetAttachPoint(AttachPointName.Muzzle);
		TrailRenderer trail = Instantiate(bulletTrail, muzzle.position, Quaternion.identity);
		var muzzleEffect = Instantiate(MuzzleFlash, muzzle.transform);
		muzzleEffect.transform.position = muzzle.position; muzzleEffect.transform.rotation = muzzle.rotation;
		StartCoroutine(SpawnTrail(trail, hitLoc));
	}

	void DoImpactEffect(RaycastHit tr)
	{
		Instantiate(hitParticle, tr.point, Quaternion.LookRotation(tr.normal));
	}


	IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitLoc)
	{
		Debug.Log("pew");
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


	void PlaySound(AudioClip sound)
	{
		if (!audioSource || !sound) return;

		audioSource.PlayOneShot(sound);
	}

	void Reload()
	{
		if (weaponAnim) { isReloading = true; weaponAnim.SetTrigger("reload"); }
		else
		{
			var ammoComp = player.GetComponent<Ammo>();
			if (!ammoComp) return;


			var clip = Mathf.RoundToInt(weaponStats.GetStat(StatType.ClipSize));

			var amount = ammoComp.TakeAmmo(weaponType, clip - currentAmmo);

			Debug.Log(ammoComp.GetAmmo(weaponType));
			currentAmmo += amount;
			Debug.Log($"Current ammo (Reload): {currentAmmo}");
			GameManager.Instance.ChangeWeapnEvent();
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

	void ReloadFinished()
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

	}

	void ShowDamage(float val, Vector3 spawnPoint)
	{

		Camera camera = Camera.main;
		if (!camera) return;
		var popup = Instantiate(GameManager.Instance.DmgPopup, spawnPoint, Quaternion.LookRotation(spawnPoint - camera.transform.position));
		popup.DisplayDamage(val);

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

