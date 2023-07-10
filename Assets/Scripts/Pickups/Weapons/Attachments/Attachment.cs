using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponStats))]
public class Attachment : MonoBehaviour
{

	Weapon myWeapon;

	WeaponStats myStats;

	// Start is called before the first frame update
	void Start()
	{
		myWeapon = gameObject.transform.parent.gameObject.GetComponent<Weapon>();

		if (!myWeapon) Destroy(gameObject);



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
