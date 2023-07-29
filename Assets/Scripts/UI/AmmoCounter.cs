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
		gameObject.SetActive(false);
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
		string wepName = "";
		string ammoString = "";
		if (!currentWeapon || !currentWeapon.isActiveAndEnabled)
		{
			wepName = "";
			ammoString = "";
			text.text = wepName + ammoString;
			return;
		}
		wepName = currentWeapon.ObjectName;
		if (!currentWeapon) { gameObject.SetActive(false); } else { gameObject.SetActive(true); }



		if (currentWeapon.GetType() == typeof(RangedWeapon) || currentWeapon.GetType().IsSubclassOf(typeof(RangedWeapon)))
		{
			var rangedWep = (RangedWeapon)currentWeapon;
			ammoString = $": {rangedWep.currentAmmo}/{player.GetComponent<Ammo>().GetAmmo(rangedWep.WeaponType)}";
		}

		if (currentWeapon.GetType() == typeof(ThrowableWeapon) || currentWeapon.GetType().IsSubclassOf(typeof(ThrowableWeapon)))
		{
			var throwable = (ThrowableWeapon)currentWeapon;
			ammoString = $" {throwable.currentCount}";
		}

		text.text = wepName + ammoString;



	}

}
