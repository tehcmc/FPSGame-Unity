using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
	[SerializeField] Weapon weapon;
	[SerializeField] Transform weaponPoint;
	Player player;
	// Start is called before the first frame update
	void Start()
	{
		if (weapon.weaponModel) _ = Instantiate(weapon.weaponModel, weaponPoint);

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		var plr = other.GetComponent<Player>();
		if (!plr) return;
		if (plr.gameObject.GetComponent<WeaponInventory>())
		{
			plr.gameObject.GetComponent<WeaponInventory>().AddWeapon(weapon);
		}


	}

}
