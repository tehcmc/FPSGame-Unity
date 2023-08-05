using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

	[SerializeReference] Attachment item;
	[SerializeField] int price = 100;

	[SerializeField] TMP_Text itemText;
	[SerializeField] TMP_Text priceText;
	[SerializeField] Button buyButton;


	Player player;
	string buttonText = string.Empty;
	public Attachment Item { get => item; set => item = value; }
	public int Price { get => price; set => price = value; }

	// Start is called before the first frame update
	private void Awake()
	{
		GameManager.Instance.WeaponChanged += ChangeUI;
		player = FindObjectOfType<Player>();
	}
	void Start()
	{

	}
	private void OnEnable()
	{
		itemText.text = Item.ObjectName;
		priceText.text = $"Price:{Price}";
		ChangeUI();


	}
	// Update is called once per frame
	void Update()
	{

	}

	public void Purchase()
	{
		if (CanPurchase())
		{
			var gun = (RangedWeapon)player.GetComponent<WeaponInventory>().CurrentWeapon;
			gun.SetupAttachment((Attachment)Item);
			player.GetComponent<PointBank>().SubtractPoints(price);

			//	ChangeUI();
		}
	}



	bool CanPurchase()
	{

		var playerPoints = player.GetComponent<PointBank>();

		if (!playerPoints.CanSubtractPoints(price)) { buttonText = "Insufficient Points"; return false; }

		var currentWeapon = (RangedWeapon)player.GetComponent<WeaponInventory>().CurrentWeapon;
		if (!currentWeapon || !Item) { buttonText = "No Weapon"; return false; }

		foreach (var attachment in currentWeapon.AttachmentDictionary)
		{
			if (attachment.Value.ObjectName == Item.ObjectName) return false;
		}

		foreach (WeaponType type in Item.ValidWeapons) //loop through all valid weapon types on current attachment
		{
			if (currentWeapon.WeaponType != type) // if current weapon is not a valid weapon type
			{
				continue; // go to next attachment
			}

			foreach (AttachPoint point in currentWeapon.AttachPoints) //if it IS a valid weapon, loop through all the valid attachment points on the weapon
			{

				if (Item.MyAttachPoint != point.AttachmentPoint)
				{
					continue; // if the weapon doesn't have the point that this attachment attaches to, move to next attachment, display it
				}
				else
				{
					buttonText = "Buy";
					return true;
				}

			}

		}
		buttonText = "Can't Attach";
		return false;
	}


	void ChangeUI()
	{
		if (!buyButton) return;
		buyButton.interactable = CanPurchase();
		buyButton.GetComponentInChildren<TMP_Text>().text = buttonText;
	}

}






























