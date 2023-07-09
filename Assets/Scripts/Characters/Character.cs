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
		health.onDamage.AddListener(TakeDamage);
		health.onZeroed.AddListener(Die);

	}
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	protected virtual void TakeDamage()
	{
		Debug.Log(gameObject.name + ": Ouch");
	}

	protected virtual void Die()
	{
		Destroy(gameObject);

	}

}
