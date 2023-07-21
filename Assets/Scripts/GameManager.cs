using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public event Action WeaponChanged;
	public static GameManager Instance;
	[SerializeField] DamagePopup dmgPopup;

	[SerializeField] List<Attachment> attachmentList = new List<Attachment>();
	[SerializeField] AudioClip headshotSound;

	public DamagePopup DmgPopup { get => dmgPopup; set => dmgPopup = value; }
	public List<Attachment> AttachmentList { get => attachmentList; set => attachmentList = value; }

	public bool debugMode { get; private set; } = false;
	public AudioClip HeadshotSound { get => headshotSound; set => headshotSound = value; }
	public bool Paused { get => paused; set => paused = value; }

	bool paused = false;

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
	private void Start()
	{

	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			SwitchDebug();
		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			PauseGame();
		}
	}
	void SwitchDebug()
	{
		debugMode = !debugMode;
	}

	void PauseGame()
	{
		Paused = !Paused;

		var player = FindObjectOfType<Player>();

		if (!player) return;
		var moveComp = player.GetComponent<FirstPersonController>();

		var inventory = player.GetComponent<WeaponInventory>();
		var weaponComp = inventory.CurrentWeapon;
		if (Paused)
		{
			if (moveComp) moveComp.enabled = false;
			if (inventory) inventory.enabled = false;
			if (weaponComp) weaponComp.enabled = false;

		}
		else
		{
			if (moveComp) moveComp.enabled = true;
			if (inventory) inventory.enabled = true;
			if (weaponComp) weaponComp.enabled = true;
		}




	}
	public void ChangeWeapnEvent()
	{
		WeaponChanged?.Invoke();
	}

}
