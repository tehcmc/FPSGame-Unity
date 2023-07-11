using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : Attachment
{
	[SerializeField] Camera playerCamera;
	bool isZoomed = false;
	[SerializeField] float zoomAmount = 20f;
	float defaultFOV;
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

		playerCamera = player.GetComponentInChildren<Camera>();
		if (playerCamera) defaultFOV = playerCamera.fieldOfView;
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
		}
		else
		{
			playerCamera.fieldOfView = defaultFOV;
		}
	}


}
