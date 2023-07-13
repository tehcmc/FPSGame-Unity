using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInventory : MonoBehaviour
{
	[SerializeField] List<Weapon> weapons = new();
	Player player;

	Weapon currentWeapon;

	public Weapon CurrentWeapon { get => currentWeapon; set => currentWeapon = value; }


	private void Awake()
	{
		player = GetComponent<Player>();
	}
	private void Start()
	{
		for (int i = 0; i < weapons.Count; i++)
		{
			weapons[i] = Instantiate(weapons[i], player.WeaponHoldPoint.transform);
			weapons[i].gameObject.SetActive(false);
		}


	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Keypad0))
		{
			SwapWeapon(0);
		}

		if (Input.GetKeyDown(KeyCode.Keypad1))
		{
			SwapWeapon(1);
		}

		if (Input.GetKeyDown(KeyCode.Keypad2))
		{
			SwapWeapon(2);
		}
	}
	public void AddWeapon(Weapon weapon)
	{

		foreach (var wep in weapons)
		{
			if (wep.WeaponName == weapon.WeaponName) return;
		}

		weapon = Instantiate(weapon, player.WeaponHoldPoint.transform);
		weapons.Add(weapon);
		weapon.gameObject.SetActive(false);


	}

	void SwapWeapon(int index)
	{
		if (index >= weapons.Count) return;
		if (!weapons[index]) return;
		if (currentWeapon == weapons[index]) return;

		if (currentWeapon)
		{
			currentWeapon.gameObject.SetActive(false);
		}



		currentWeapon = weapons[index];
		Debug.Log(currentWeapon.WeaponName);
		currentWeapon.gameObject.SetActive(true);




	}



}
