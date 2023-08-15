using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireModeDisplay : MonoBehaviour
{
	Player player;
	RangedWeapon weapon;
	TMP_Text fireMode;

	private void Awake()
	{
		fireMode = GetComponent<TMP_Text>();
	}
	void Start()
	{
		player = FindObjectOfType<Player>();
		GameManager.Instance.WeaponChanged += ChangeDisplay;
		ChangeDisplay();
	}

	// Update is called once per frame
	void Update()
	{

	}
	void ChangeDisplay()
	{
		weapon = player.GetComponent<WeaponInventory>().CurrentWeapon as RangedWeapon;
		if (!weapon) { fireMode.text = ""; return; }

		fireMode.text = weapon.WeaponFiremode.ToString();
	}

}
