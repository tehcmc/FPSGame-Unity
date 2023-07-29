using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuilding : MonoBehaviour
{
	Player player;
	[SerializeField] GameObject ShopUI;

	bool shopVisible = false;
	// Start is called before the first frame update
	void Start()
	{
		ShopUI.SetActive(shopVisible);
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
		ShopUI.SetActive(shopVisible);
	}


}
