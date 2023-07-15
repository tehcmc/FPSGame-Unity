using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	[SerializeField] DamagePopup dmgPopup;
	Camera mainCam;
	public DamagePopup DmgPopup { get => dmgPopup; set => dmgPopup = value; }
	public Camera MainCam { get => mainCam; set => mainCam = value; }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);

		}
		else
		{
			Instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		MainCam = Camera.main;

	}

	// Update is called once per frame
	void Update()
	{

	}
}
