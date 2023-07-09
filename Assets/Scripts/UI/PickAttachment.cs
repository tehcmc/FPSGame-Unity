using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAttachment : MonoBehaviour
{
	[SerializeField] Attachment attachment;
	Weapon weapon;


	Player player;

	// Start is called before the first frame update
	void Start()
	{
		player = FindObjectOfType<Player>();

		weapon = player.gameObject.GetComponentInChildren<Weapon>();



	}

	public void AttachToWeapon()
	{
		Debug.Log("Function");
		if (!weapon) return;
		Debug.Log("Weapon found");
		if (!attachment) return;
		Debug.Log("Attachment found");
		weapon.SetupAttachment(attachment);


	}


	// Update is called once per frame
	void Update()
	{

	}
}
