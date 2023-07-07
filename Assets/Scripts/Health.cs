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

	public void ModifyHealth(float val)
	{
		if (val == 0) return;

		currentHealth += val;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		Debug.Log(currentHealth);
		if (currentHealth <= 0)
		{
			Die();
			return;
		}

	}

	void Die()
	{
		if (gameObject.GetComponent<Character>())
		{
			//character die
		}

		Destroy(gameObject);
	}
	// Update is called once per frame
	void Update()
	{

	}
}
