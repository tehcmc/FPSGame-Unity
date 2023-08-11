using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Flashlight : Attachment
{
	[SerializeField] GameObject light;
	[SerializeField] AudioClip activateSound;

	AudioSource audioSource;
	bool isOn;

	// Start is called before the first frame update
	protected override void Start()
	{
		audioSource = GetComponent<AudioSource>();
		light.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.F))
		{

			SwitchLight();

		}

	}

	void SwitchLight()
	{
		isOn = !isOn;

		if (isOn)
		{
			audioSource.PlayOneShot(activateSound);
			light.SetActive(true);
		}
		else
		{
			audioSource.PlayOneShot(activateSound);
			light.SetActive(false);
		}

	}
}
