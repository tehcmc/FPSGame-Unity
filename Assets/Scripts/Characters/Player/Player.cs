using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(DeathHandler))]
public class Player : Character
{
	public event Action playerHealthChanged;
	[SerializeField] Transform weaponHoldPoint;
	[SerializeField] Canvas hitCanvas;
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
		health.OnHeal += Healed;
	}

	protected override void TakeDamage()
	{
		base.TakeDamage();
		playerHealthChanged.Invoke();
		if (hitCanvas) hitCanvas.GetComponent<Animator>().SetTrigger("playHit");
	}

	protected void Healed()
	{
		playerHealthChanged.Invoke();
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
