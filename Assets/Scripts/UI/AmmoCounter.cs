using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
	// Start is called before the first frame update

	Player player;
	RangedWeapon currentWeapon;
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

	}

	void SetAmmoCount()
	{

		if (!player) return;

		currentWeapon = player.GetComponent<WeaponInventory>().CurrentWeapon;
		if (!currentWeapon) { gameObject.SetActive(false); } else { gameObject.SetActive(true); }

		if (!currentWeapon) return;

		text.text = $"{currentWeapon.WeaponName}: {currentWeapon.currentAmmo}/{player.GetComponent<Ammo>().GetAmmo(currentWeapon.WeaponType)}";
	}

}
