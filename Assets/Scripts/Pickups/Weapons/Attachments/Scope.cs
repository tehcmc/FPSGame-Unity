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
	private void OnEnable()
	{

		myStats = GetComponent<WeaponStats>();
		Debug.Log($"Enabled {name}");

		AddStats();

	}
	private void OnDisable()
	{
		if (isZoomed) Zoom();
		RemoveStats();
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
	protected override void AddStats()
	{
		if (myStats) myWeapon.WeaponStats.BaseDamage += myStats.BaseDamage;
		if (myStats) myWeapon.WeaponStats.FireRate += myStats.FireRate;
	}
	protected override void RemoveStats()
	{
		if (myStats) myWeapon.WeaponStats.BaseDamage -= myStats.BaseDamage;
		if (myStats) myWeapon.WeaponStats.FireRate -= myStats.FireRate;
	}

}
