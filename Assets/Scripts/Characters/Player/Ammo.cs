using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[Serializable]
public class AmmoType
{
	[SerializeField] WeaponType weaponType;
	[SerializeField] int amount;



	AmmoType(WeaponType type, int val)
	{
		this.WeaponType = type;
		this.Amount = val;
	}

	public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
	public int Amount { get => amount; set => amount = value; }

}
public class Ammo : MonoBehaviour
{


	[SerializeField] List<AmmoType> ammoTypes = new();

	IDictionary<WeaponType, AmmoType> ammoTable = new Dictionary<WeaponType, AmmoType>();

	private void Awake()
	{
		foreach (var type in ammoTypes)
		{
			ammoTable.Add(type.WeaponType, type);
		}
	}
	public virtual int GetAmmo(WeaponType key)
	{
		if (!ammoTable.ContainsKey(key)) return 0;


		return ammoTable[key].Amount;
	}

	public int TakeAmmo(WeaponType key, int amount)
	{
		if ((ammoTable == null)) return 0;

		var available = ammoTable[key].Amount;

		amount = Mathf.Min(available, amount);



		SetAmmo(key, available - amount);
		return amount;
	}

	public void GiveAmmo(WeaponType key, int amount)
	{
		if (!ammoTable.ContainsKey(key)) return;

		var current = ammoTable[key].Amount;

		SetAmmo(key, current += amount);

	}


	public void SetAmmo(WeaponType key, int amount)
	{
		ammoTable[key].Amount = amount;

		GameManager.Instance.ChangeWeapnEvent();
	}

	public void DestroyAmmoTable()
	{
		ammoTable.Clear();
	}


}
