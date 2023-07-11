using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponStats))]
public class Attachment : MonoBehaviour
{

	protected Weapon myWeapon;

	protected WeaponStats myStats;

	[SerializeField] AttachPointName myAttachPoint;

	public AttachPointName MyAttachPoint { get => myAttachPoint; set => myAttachPoint = value; }

	protected virtual void Awake()
	{


	}
	protected virtual void OnEnable()
	{
		Debug.Log("enabled");


		myStats = GetComponent<WeaponStats>();
		myStats.StatDictionary.ContainsKey(StatType.HorizontalRecoil);
		myStats.enabled = false;
		myStats.enabled = true;
		myWeapon = gameObject.transform.parent.gameObject.GetComponent<Weapon>();
		AddStats();
	}
	protected virtual void OnDisable()
	{

		RemoveStats();
		myStats = null;
		myWeapon = null;

	}
	void Start()
	{
		if (!myWeapon) Destroy(gameObject);
	}

	protected void AddStats()
	{
		if (myStats.StatDictionary == null) return;
		var test = 1;
		test = 2;


		foreach (var stat in myStats.StatDictionary)
		{
			Debug.Log($"Name: {stat.Key} Val: {stat.Value}");
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


	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Debug.Log("Attachment Function!");
		}
	}
}
