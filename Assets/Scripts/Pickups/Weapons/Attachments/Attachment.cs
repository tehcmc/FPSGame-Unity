using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponStats))]
public class Attachment : MonoBehaviour
{

	protected Weapon myWeapon;

	protected WeaponStats myStats;

	protected virtual void Awake()
	{

		myStats = GetComponent<WeaponStats>();
		myWeapon = gameObject.transform.parent.gameObject.GetComponent<Weapon>();
	}

	void Start()
	{
		if (!myWeapon) Destroy(gameObject);
	}

	protected virtual void AddStats()
	{

	}
	protected virtual void RemoveStats()
	{

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
