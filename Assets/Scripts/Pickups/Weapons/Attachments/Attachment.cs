using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponStats))]
public class Attachment : MonoBehaviour
{



	[SerializeField] List<WeaponType> validWeapons = new List<WeaponType>();
	[SerializeField] AttachPointName myAttachPoint;



	protected WeaponStats myStats;
	protected RangedWeapon myWeapon;

	public AttachPointName MyAttachPoint { get => myAttachPoint; set => myAttachPoint = value; }

	protected virtual void Awake()
	{
		myStats = GetComponent<WeaponStats>();
		myWeapon = gameObject.transform.parent.gameObject.GetComponent<RangedWeapon>();
	}
	protected virtual void OnEnable()
	{

		AddStats();
	}
	protected virtual void OnDisable()
	{

		RemoveStats();


	}
	void Start()
	{
		if (!myWeapon) Destroy(gameObject);
	}

	protected void AddStats()
	{
		if (myStats.StatDictionary == null) return;

		foreach (var stat in myStats.StatDictionary)
		{
			//	Debug.Log(gameObject.name);
			//	Debug.Log($"Name: {stat.Key} Val: {stat.Value}");
			myWeapon.WeaponStats.ModifyStat(stat.Key, stat.Value);
		}
	}


	protected void RemoveStats()
	{

		foreach (var stat in myStats.StatDictionary)
		{
			myWeapon.WeaponStats.ModifyStat(stat.Key, -stat.Value);
		}
	}

	public bool CheckIfValid(WeaponType weptype)
	{
		foreach (var type in validWeapons)
		{
			if (type == weptype) return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Debug.Log("Attachment Function!");
		}
	}
	private void OnDestroy()
	{
		myStats = null;
		myWeapon = null;
	}


}
