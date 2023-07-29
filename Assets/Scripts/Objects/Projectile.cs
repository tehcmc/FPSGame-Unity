using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	[SerializeField] float projectileSpeed;
	[SerializeField] float lifeTime = 10f;



	Rigidbody rb;
	protected float currentTime = 0;

	public Rigidbody Rb { get => rb; set => rb = value; }
	public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }

	protected virtual void Awake()
	{
		Rb = GetComponent<Rigidbody>();
	}
	protected virtual void OnEnable()
	{

	}

	protected virtual void OnDisable()
	{

	}


	void Start()
	{

	}


	protected virtual void Update()
	{
		currentTime += Time.deltaTime;
		if (LifetimeExpired()) Die();



	}
	public void ShootProjectile(Vector3 direction)
	{
		rb.AddRelativeForce(direction);
	}



	bool LifetimeExpired()
	{
		if (currentTime < lifeTime) return false;

		return true;
	}
	protected virtual void OnCollisionEnter(Collision collision)
	{
		Destroy(gameObject);
	}

	protected virtual void Die()
	{

		if (!gameObject.activeInHierarchy) return;
		Destroy(gameObject);
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
