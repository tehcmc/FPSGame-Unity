using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{


	public event Action OnHeal;
	public event Action OnDamage;
	public event Action OnZeroed;


	// Start is called before the first frame update
	[SerializeField] float maxHealth = 100f;
	float currentHealth;

	private void Awake()
	{
		currentHealth = maxHealth;
	}
	void Start()
	{

	}

	void ModifyHealth(float val)
	{
		if (val == 0) return;

		currentHealth += val;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		//	Debug.Log(currentHealth);

		if (currentHealth <= 0)
		{
			HealthZeroed();
			return;
		}

	}

	public void DamageHealth(float val)
	{
		if (val == 0) return;
		ModifyHealth(-Mathf.Abs(val));
		OnDamage?.Invoke();
	}



	public void Heal(float val)
	{
		if (val == 0) return;
		ModifyHealth(Mathf.Abs(val));
		OnHeal?.Invoke();
	}

	void HealthZeroed()
	{
		OnZeroed?.Invoke();
	}
	// Update is called once per frame
	void Update()
	{

	}
}
