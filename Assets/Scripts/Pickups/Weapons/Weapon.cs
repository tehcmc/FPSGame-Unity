using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
	Melee,
	Pistol,
	SMG,
	Rifle,
	Shotgun,
	Sniper,
	Special
}


[RequireComponent(typeof(AudioComponent))]
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
	protected AudioComponent audioComponent;

	[Header("Values")]
	[SerializeField] protected string weaponName;
	[SerializeField] protected WeaponType weaponType;
	protected Animator weaponAnim;


	protected Camera cam;
	protected Player player;

	public GameObject weaponModel;
	public WeaponType WeaponType { get => weaponType; set => weaponType = value; }

	public string WeaponName { get => weaponName; set => weaponName = value; }
	protected virtual void Awake()
	{
		audioComponent = GetComponent<AudioComponent>();
		weaponAnim = GetComponent<Animator>();
		weaponAnim.keepAnimatorStateOnDisable = true;
	}
	protected virtual void OnEnable()
	{
		Debug.Log("Wep enable");

		GameManager.Instance.ChangeWeapnEvent();

	}
	protected virtual void OnDisable()
	{

		GameManager.Instance.ChangeWeapnEvent();
	}
	protected virtual void Start()
	{
		player = FindObjectOfType<Player>();
		if (!player) Destroy(gameObject);
		cam = player.GetComponentInChildren<Camera>();

	}

	protected void PlaySound(string name)
	{
		if (!audioComponent || name == null) return;
		audioComponent.PlaySound(name);
	}

	protected void ShowDamage(float val, Vector3 spawnPoint)
	{
		float roundedVal = MathF.Round(val * 100.0f) * 0.01f;

		Camera camera = Camera.main;
		if (!camera) return;
		var popup = Instantiate(GameManager.Instance.DmgPopup, spawnPoint, Quaternion.LookRotation(spawnPoint - camera.transform.position));
		popup.DisplayDamage(roundedVal);

	}

}
