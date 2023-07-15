using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
	[SerializeField] TMP_Text text;
	// Start is called before the first frame update
	void Start()
	{
		if (!text) Destroy(gameObject);
	}

	public void DisplayDamage(float damage)
	{
		text.text = damage.ToString();
	}
}
