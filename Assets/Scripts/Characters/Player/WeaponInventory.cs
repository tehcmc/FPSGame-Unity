using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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

	private void Update()
	{
		Debug.Log("Count = " + weapons.Count);
		if (Input.GetKeyDown(KeyCode.L))
		{

			SwapWeapon(0);


		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			SwapWeapon(1);
		}

	}
	void AddWeapon(Weapon weapon)
	{
		weapons.Add(weapon);

	}

	void SwapWeapon(int index)
	{
		if (!weapons[index]) return;


		if (currentWeapon)
		{

			Debug.Log("contains");
			for (int i = 0; i < weapons.Count; i++)
			{
				if (weapons[i].WeaponName == currentWeapon.WeaponName)
				{
					Debug.Log("found");
				}

			}


			currentWeapon.gameObject.SetActive(false);
			Destroy(currentWeapon.gameObject);
			currentWeapon = null;
		}

		if (!weapons[index]) return;

		currentWeapon = Instantiate(weapons[index], player.WeaponHoldPoint.transform);




	}



}
