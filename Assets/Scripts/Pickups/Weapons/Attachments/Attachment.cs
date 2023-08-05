using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponStats))]
public class Attachment : BaseObject
{



	[SerializeField] List<WeaponType> validWeapons = new List<WeaponType>();
	[SerializeField] AttachPointName myAttachPoint;



	protected WeaponStats myStats;
	protected RangedWeapon myWeapon;

	public AttachPointName MyAttachPoint { get => myAttachPoint; set => myAttachPoint = value; }
	public List<WeaponType> ValidWeapons { get => validWeapons; set => validWeapons = value; }
	Player player;
	RangedWeapon playerWeapon;
	protected virtual void Awake()
	{
		myStats = GetComponent<WeaponStats>();
		myWeapon = gameObject.transform.parent.gameObject.GetComponent<RangedWeapon>();
		player = FindObjectOfType<Player>();
		playerWeapon = player.GetComponent<WeaponInventory>().CurrentWeapon as RangedWeapon;
	}
	protected virtual void OnEnable()
	{

		AddStats();
	}
	protected virtual void OnDisable()
	{

		RemoveStats();


	}

	public virtual void Start()
	{

	}

	protected void AddStats()
	{
		if (!myWeapon) return;
		if (myStats.StatDictionary == null) return;

		foreach (var stat in myStats.StatDictionary)
		{
			myWeapon.WeaponStats.ModifyStat(stat.Key, stat.Value);
		}
	}


	protected void RemoveStats()
	{
		if (!myWeapon) return;
		if (myStats.StatDictionary == null) return;

		foreach (var stat in myStats.StatDictionary)
		{
			myWeapon.WeaponStats.ModifyStat(stat.Key, -stat.Value);
		}
	}

	public bool CheckIfValid(WeaponType weptype)
	{

		foreach (var type in ValidWeapons)
		{
			if (type == weptype) return true;
		}

		return false;
	}

	public bool CheckIfAttached(RangedWeapon weapon)
	{
		if (weapon.AttachmentDictionary[MyAttachPoint].ObjectName == ObjectName) return true;
		return false;
	}
	private void OnDestroy()
	{
		myStats = null;
		myWeapon = null;
	}

	public override bool CanPurchase()
	{
		if (!playerWeapon) return false;

		if (!CheckIfValid(playerWeapon.WeaponType)) return false;

		if (CheckIfAttached(playerWeapon)) return false;

		return true;
	}

	public override void PurchaseItem()
	{
		if (!myWeapon) return;

		playerWeapon.SetupAttachment(this);
	}
}
