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


		weapon = Instantiate(weapon, player.WeaponHoldPoint.transform); //create weapon gameobject


		weapons.Add(weapon); //add created weapon to weapon list
		weapon.gameObject.SetActive(false);//set weapon to inactive

		SwapWeapon(weapons.Count - 1); //swap to last entry in weapon list (*should* be the newly created weapon)

		var sortedList = weapons.OrderByDescending(x => (int)(x.WeaponType)).ToList(); //sort weapon list by weapon type
		sortedList.Reverse(); // reverse sorted list to get properly sorted list. Based on order of weaponType enum

		weapons = sortedList; // set weapon list to the sorted list

	}

	public bool CanAddToInventory(Weapon weapon)
	{
		return CanPickUp(weapon.ObjectName);

	}

	void SwapWeapon(int index)
	{
		if (index >= weapons.Count) return; // if index is outside of current weapon list size return

		if (!weapons[index]) { weapons.RemoveAt(index); GameManager.Instance.ChangeWeapnEvent(); return; } // if the weapon at the current index value does not exist, delete this index entry

		if (currentWeapon == weapons[index]) return;

		if (currentWeapon)
		{
			currentWeapon.gameObject.SetActive(false); // deactivate current weapon
		}

		currentWeapon = weapons[index]; // set current weapon to the weapon at the index entry

		currentWeapon.gameObject.SetActive(true); //activate new current weapon
	}

	public virtual bool CanPickUp(string name) //check if weapon with given name can be picked up
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
