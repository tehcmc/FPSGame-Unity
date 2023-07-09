using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
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

		Debug.Log(currentHealth);

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
	}

	public void Heal(float val)
	{
		if (val == 0) return;
		ModifyHealth(Mathf.Abs(val));
	}

	void HealthZeroed()
	{
		var character = gameObject.GetComponent<Character>();
		if (character)
		{
			character.Die();
		}
		else
		{
			Destroy(gameObject);
		}

	}
	// Update is called once per frame
	void Update()
	{

	}
}
