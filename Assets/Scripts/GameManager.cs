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
		paused = !paused;

		var player = FindObjectOfType<Player>();

		if (!player) return;
		var moveComp = player.GetComponent<FirstPersonController>();
		var weaponComp = player.GetComponent<WeaponInventory>().CurrentWeapon;

		if (paused)
		{
			if (moveComp) moveComp.enabled = false;
			if (weaponComp) weaponComp.enabled = false;
		}
		else
		{
			if (moveComp) moveComp.enabled = true;
			if (weaponComp) weaponComp.enabled = true;
		}




	}
	public void ChangeWeapnEvent()
	{
		WeaponChanged?.Invoke();
	}

}
