using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	[SerializeField] float projectileSpeed;
	[SerializeField] float lifeTime = 10f;

	Rigidbody rb;
	float currentTime;

	public Rigidbody Rb { get => rb; set => rb = value; }

	protected virtual void Awake()
	{
		Rb = GetComponent<Rigidbody>();
	}
	protected virtual void OnEnable()
	{
		Rb.AddRelativeForce(0, 0, projectileSpeed);
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
}
