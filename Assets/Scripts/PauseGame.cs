using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
	Player player;
	StarterAssets.StarterAssetsInputs playerInput;

	bool gamePaused = false;


	void Start()
	{
		player = FindObjectOfType<Player>();

		if (!player) { Destroy(gameObject); return; }
		playerInput = player.GetComponent<StarterAssetsInputs>();


	}
	void Pause()
	{
		gamePaused = !gamePaused;

		if (gamePaused)
		{
			Cursor.lockState = CursorLockMode.None;

		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;

		}



		//		playerInput.cursorLocked = false;
		//	playerInput.cursorInputForLook = false;
		//		playerInput.SetCursorState(playerInput.cursorLocked);
	}
	void UnPause()
	{
		gamePaused = false;



		//		playerInput.cursorLocked = true;
		//		playerInput.cursorInputForLook = true;
		//		playerInput.SetCursorState(playerInput.cursorLocked);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{

			Pause();

		}
	}


}
