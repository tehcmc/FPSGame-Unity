using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
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

		var oldHealth = currentHealth;
		currentHealth += val;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		Debug.Log($"{oldHealth} + {val} = {currentHealth}");

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
		ShowDamage(val);
		OnDamage?.Invoke();
	}

	void ShowDamage(float val)
	{
		Camera camera = Camera.main;
		if (!camera) return;
		Vector3 randPos = gameObject.transform.position;

		randPos.x += (Random.Range(-1, 1));
		randPos.y += (Random.Range(-1, 1));
		randPos.z += (Random.Range(-1, 1));
		var popup = Instantiate(GameManager.Instance.DmgPopup, randPos, Quaternion.LookRotation(gameObject.transform.position - camera.transform.position));
		popup.DisplayDamage(val);
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
