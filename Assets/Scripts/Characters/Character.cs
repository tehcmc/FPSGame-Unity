using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Health))]
public class Character : MonoBehaviour
{
	Health health;
	// Start is called before the first frame update
	protected virtual void Awake()
	{

		health = GetComponent<Health>();

	}
	private void OnEnable()
	{

		health.OnDamage += TakeDamage;
		health.OnZeroed += Die;
	}
	private void OnDisable()
	{
		health.OnDamage -= TakeDamage;
		health.OnZeroed -= Die;
	}

	void Start()
	{

	}

	void Update()
	{

	}

	protected virtual void TakeDamage()
	{
		Debug.Log(gameObject.name + ": Ouch");
	}

	protected virtual void Die()
	{
		// base class does nothing!
	}

}
