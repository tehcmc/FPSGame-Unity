using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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


		if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{


			if (Input.GetAxis("Mouse ScrollWheel") > 0 && indexEntry < weapons.Count - 1)
			{
				indexEntry++;
			}

			if (Input.GetAxis("Mouse ScrollWheel") < 0 && indexEntry > 0)
			{
				indexEntry--;
			}

			SwapWeapon(indexEntry);

		}
	}

	public void AddWeapon(Weapon weapon)
	{

		if (!CanAddToInventory(weapon)) return;


		weapon = Instantiate(weapon, player.WeaponHoldPoint.transform);
		weapons.Add(weapon);
		weapon.gameObject.SetActive(false);
		SwapWeapon(weapons.Count - 1);
		var sortedList = weapons.OrderByDescending(x => (int)(x.WeaponType)).ToList();
		sortedList.Reverse();
		weapons = sortedList;
	}

	public bool CanAddToInventory(Weapon weapon)
	{
		return CanPickUp(weapon.ObjectName);

	}

	void SwapWeapon(int index)
	{
		if (index >= weapons.Count) return;
		if (!weapons[index]) { weapons.RemoveAt(index); GameManager.Instance.ChangeWeapnEvent(); return; }
		if (currentWeapon == weapons[index]) return;

		if (currentWeapon)
		{
			currentWeapon.gameObject.SetActive(false);
		}

		currentWeapon = weapons[index];
		currentWeapon.gameObject.SetActive(true);
	}

	public virtual bool CanPickUp(string name)
	{
		if (name == null) return false;



		foreach (var weapon in weapons)
		{
			if (weapon.ObjectName == name) return false;
		}

		Debug.Log("add");
		return true;
	}

}
