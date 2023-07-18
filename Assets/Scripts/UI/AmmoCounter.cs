using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
	// Start is called before the first frame update

	Player player;
	Weapon currentWeapon;
	TMP_Text text;
	void Start()
	{
		text = GetComponent<TMP_Text>();
		player = FindObjectOfType<Player>();
		if (!player || !text) Destroy(gameObject);
		GameManager.Instance.WeaponChanged += SetAmmoCount;
		SetAmmoCount();
	}

	// Update is called once per frame
	void Update()
	{
		if (this == null)
		{
			Destroy(gameObject);
		}
	}

	void SetAmmoCount()
	{
		if (!player) return;
		currentWeapon = player.GetComponent<WeaponInventory>().CurrentWeapon;
		if (!currentWeapon) return;

		if (!currentWeapon) { gameObject.SetActive(false); } else { gameObject.SetActive(true); }
		string ammoString = "";
		if (currentWeapon.GetType() == typeof(RangedWeapon) || currentWeapon.GetType().IsSubclassOf(typeof(RangedWeapon)))
		{
			var rangedWep = (RangedWeapon)currentWeapon;
			ammoString = $": {rangedWep.currentAmmo}/{player.GetComponent<Ammo>().GetAmmo(rangedWep.WeaponType)}";
		}




		if (!currentWeapon) return;



		text.text = $"{currentWeapon.WeaponName}" + ammoString;
	}

}
