using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Item
{

	[SerializeField] Attachment attachmentItem;
	[SerializeField] int price = 100;

	public Attachment AttachmentItem { get => attachmentItem; set => attachmentItem = value; }
	public int Price { get => price; set => price = value; }

}
public class ShopBuilding : MonoBehaviour
{
	Player player;
	ShopMenu shopMenu;
	[SerializeField] List<Item> items = new();



	bool shopVisible = false;
	// Start is called before the first frame update

	void Start()
	{
		shopMenu = FindObjectOfType<ShopMenu>(includeInactive: true);
		shopMenu.gameObject.SetActive(shopVisible);


	}

	// Update is called once per frame
	void Update()
	{
		if (player && Input.GetKeyDown(KeyCode.E))
		{
			ShowShopUI();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.GetComponent<Player>()) return;

		player = other.gameObject.GetComponent<Player>();



	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.GetComponent<Player>()) return;
		player = null;
		if (shopVisible) ShowShopUI();
	}
	void ShowShopUI()
	{
		shopVisible = !shopVisible;

		if (shopVisible)
		{
			shopMenu.Items = items;
		}
		else
		{

		}
		shopMenu.gameObject.SetActive(shopVisible);
	}


}
