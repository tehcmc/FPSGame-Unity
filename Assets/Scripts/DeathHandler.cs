using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{

	[SerializeField] Canvas gameOverCanvas;
	// Start is called before the first frame update
	void Start()
	{
		gameOverCanvas.enabled = false;

	}


	public void ShowCanvas()
	{

		if (!gameOverCanvas) return;



		gameOverCanvas.enabled = true;
		Cursor.lockState = CursorLockMode.None;
	}


	// Update is called once per frame
	void Update()
	{

	}
}
