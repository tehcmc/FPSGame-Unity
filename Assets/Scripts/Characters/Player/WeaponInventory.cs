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
	int indexEntry = 0;
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
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (indexEntry >= weapons.Count - 1)
			{
				indexEntry = 0;

			}
			else
			{
				indexEntry++;
			}

			SwapWeapon(indexEntry);
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

		var sortedList = weapons.OrderByDescending(x => (int)(x.WeaponType)).ToList();
		sortedList.Reverse();
		weapons = sortedList;
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
