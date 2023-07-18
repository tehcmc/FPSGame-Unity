using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickAttachment : MonoBehaviour
{
	[SerializeField] Attachment attachment;
	RangedWeapon weapon;


	Player player;
	TMP_Text text;
	// Start is called before the first frame update
	void Start()
	{
		player = FindObjectOfType<Player>();
		text = GetComponentInChildren<TMP_Text>();


		text.text = attachment.MyAttachPoint.ToString();


	}

	public void AttachToWeapon()
	{
		weapon = (RangedWeapon)player.gameObject.GetComponent<WeaponInventory>().CurrentWeapon;

		if (!weapon) return;
		if (!attachment) return;

		weapon.SetupAttachment(attachment);


	}


	// Update is called once per frame
	void Update()
	{

	}
}
