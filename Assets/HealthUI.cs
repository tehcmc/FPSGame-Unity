using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
	[SerializeField] TMP_Text healthText;
	Player player;
	// Start is called before the first frame update
	void Start()
	{

		player = FindObjectOfType<Player>();
		player.playerHealthChanged += UpdateHealth;

		UpdateHealth();
	}

	void UpdateHealth()
	{
		healthText.text = player.GetComponent<Health>().currentHealth.ToString();
	}
	// Update is called once per frame
	void Update()
	{

	}
}
