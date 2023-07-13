using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : Attachment
{
	[SerializeField] float zoomAmount = 20f;
	[SerializeField] float mouseDampen = 1f;


	Camera playerCamera;
	FirstPersonController playerController;
	bool isZoomed = false;
	float defaultFOV;
	float defaultMouseSens;
	// Start is called before the first frame update
	// 	
	protected override void Awake()
	{
		base.Awake();

	}
	protected override void OnEnable()
	{
		base.OnEnable();


	}
	protected override void OnDisable()
	{
		if (isZoomed) Zoom();
		base.OnDisable();
	}

	void Start()
	{
		var player = FindObjectOfType<Player>();
		if (!player) { Destroy(gameObject); return; }
		playerController = player.GetComponent<FirstPersonController>();
		playerCamera = player.GetComponentInChildren<Camera>();
		if (playerCamera) defaultFOV = playerCamera.fieldOfView;
		defaultMouseSens = playerController.RotationSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Zoom();
		}
	}
	void Zoom()
	{
		if (!playerCamera) return;

		isZoomed = !isZoomed;

		if (isZoomed)
		{
			playerCamera.fieldOfView = defaultFOV - zoomAmount;
			playerController.RotationSpeed = defaultMouseSens / mouseDampen;
		}
		else
		{
			playerCamera.fieldOfView = defaultFOV;
			playerController.RotationSpeed = defaultMouseSens;
		}
	}


}
