using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemoveAttacchment : MonoBehaviour
{
	[SerializeField] AttachPointName attachmentPoint;
	Weapon weapon;


	Player player;

	TMP_Text text;

	void Start()
	{
		text = GetComponentInChildren<TMP_Text>();
		player = FindObjectOfType<Player>();


		text.text = attachmentPoint.ToString().Normalize();


	}

	public void RemoveFromWeapon()
	{
		weapon = player.GetComponent<WeaponInventory>().CurrentWeapon;
		if (!weapon) return;
		weapon.RemoveAttachment(attachmentPoint);
	}


	// Update is called once per frame
	void Update()
	{

	}
}
