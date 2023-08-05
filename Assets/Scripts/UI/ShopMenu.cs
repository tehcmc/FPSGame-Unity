using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
	List<Item> items = new List<Item>();

	public List<Item> Items { get => items; set => items = value; }
	[SerializeField] Transform contentField;
	[SerializeField] ShopItem shopItem;

	private void OnEnable()
	{
		foreach (var item in items)
		{
			ShopItem uiItem = Instantiate(shopItem, contentField.transform);
			uiItem.Item = item.AttachmentItem;
			uiItem.Price = item.Price;
			uiItem.gameObject.SetActive(true);

		}

	}
	private void OnDisable()
	{

		foreach (Transform child in contentField.transform)
		{
			Destroy(child.gameObject);
		}

	}

}
