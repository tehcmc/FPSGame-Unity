using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickAttachment : MonoBehaviour
{
	[SerializeField] Attachment attachment;
	Weapon weapon;


	Player player;
	TMP_Text text;
	// Start is called before the first frame update
	void Start()
	{
		player = FindObjectOfType<Player>();
		text = GetComponentInChildren<TMP_Text>();
		weapon = player.gameObject.GetComponentInChildren<Weapon>();


		text.text = attachment.MyAttachPoint.ToString();


	}

	public void AttachToWeapon()
	{
		if (!weapon) return;
		if (!attachment) return;

		weapon.SetupAttachment(attachment);


	}


	// Update is called once per frame
	void Update()
	{

	}
}
