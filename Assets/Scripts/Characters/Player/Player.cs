using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(DeathHandler))]
public class Player : Character
{
	[SerializeField] Transform weaponHoldPoint;
	DeathHandler deathHandler;
	Ammo ammoBox;

	AudioSource audioSource;
	public Transform WeaponHoldPoint { get => weaponHoldPoint; set => weaponHoldPoint = value; }
	public AudioSource AudioSource { get => audioSource; set => audioSource = value; }

	// Start is called before the first frame update

	protected override void Awake()
	{
		base.Awake();
		AudioSource = GetComponent<AudioSource>();
		deathHandler = GetComponent<DeathHandler>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	protected override void Die()
	{
		deathHandler.ShowCanvas();
		gameObject.SetActive(false);
		base.Die();
	}
}
