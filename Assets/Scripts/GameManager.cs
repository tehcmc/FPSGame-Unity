using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public event Action WeaponChanged;
	public static GameManager Instance;
	[SerializeField] DamagePopup dmgPopup;


	public DamagePopup DmgPopup { get => dmgPopup; set => dmgPopup = value; }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);

		}
		else
		{
			Instance = this;
		}
	}

	public void ChangeWeapnEvent()
	{
		WeaponChanged?.Invoke();
	}

}
