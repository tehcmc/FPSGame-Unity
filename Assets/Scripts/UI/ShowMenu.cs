using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour
{
	bool isVisible = true;
	[SerializeField] Canvas menu;

	// Start is called before the first frame update
	void Start()
	{
		menu.enabled = true;
	}







	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Debug.Log("show menu");
			ToggleVisibility();
		}
	}

	void ToggleVisibility()
	{
		isVisible = !isVisible;
		if (isVisible)
		{
			Debug.Log("T");
			menu.enabled = true;
		}
		else
		{
			Debug.Log("F");
			menu.enabled = false;
		}


	}

}
