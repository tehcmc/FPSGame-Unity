using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponStats))]
public class Attachment : MonoBehaviour
{

	protected Weapon myWeapon;

	protected WeaponStats myStats;

	bool statsLoaded = false;

	protected virtual void Awake()
	{


	}
	protected virtual void OnEnable()
	{
		Debug.Log("enabled");


		myStats = GetComponent<WeaponStats>();
		myStats.enabled = false;
		myStats.enabled = true;
		myWeapon = gameObject.transform.parent.gameObject.GetComponent<Weapon>();
		var nam = myWeapon.name;
		AddStats();
	}
	protected virtual void OnDisable()
	{

		if (statsLoaded) RemoveStats();
		myStats = null;
		myWeapon = null;

	}
	void Start()
	{
		if (!myWeapon) Destroy(gameObject);
		if (!statsLoaded)
		{
			AddStats();
		}

	}

	protected void AddStats()
	{
		if (myStats.StatDictionary == null) return;

		foreach (var stat in myStats.StatDictionary)
		{
			Debug.Log($"Name: {stat.Key} Val: {stat.Value}");
			myWeapon.WeaponStats.ModifyStat(stat.Key, stat.Value);
		}
	}
	protected void RemoveStats()
	{
		statsLoaded = false;
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
