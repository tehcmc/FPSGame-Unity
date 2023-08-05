using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour
{
	bool isVisible = true;
	[SerializeField] Canvas menu;


	void Start()
	{
		menu.enabled = true;
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			ToggleVisibility();
		}
	}

	void ToggleVisibility()
	{
		isVisible = !isVisible;
		if (isVisible)
		{
			menu.enabled = true;
		}
		else
		{
			menu.enabled = false;
		}


	}

}
