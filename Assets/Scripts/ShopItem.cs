using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{

	[SerializeReference] BaseObject item;
	[SerializeField] int price = 100;

	[SerializeField] TMP_Text itemText;
	[SerializeField] TMP_Text priceText;

	BaseObject itm = new BaseObject();
	// Start is called before the first frame update
	private void Awake()
	{

	}
	void Start()
	{
		if (!item) Destroy(gameObject);
		item = Instantiate(item);
		item.gameObject.SetActive(false);
	}
	private void OnEnable()
	{
		itemText.text = item.ObjectName;
		priceText.text = $"Price:{price}";
		item.gameObject.SetActive(true);
		if (!item.CanPurchase())
		{
			gameObject.SetActive(false);
		}
		item.gameObject.SetActive(false);
	}
	// Update is called once per frame
	void Update()
	{

	}

	public void Purchase()
	{
		item.gameObject.SetActive(true);
		item.PurchaseItem();
		if (!item.CanPurchase())
		{
			gameObject.SetActive(false);
		}
		item.gameObject.SetActive(false);

	}
}
