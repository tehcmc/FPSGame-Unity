using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserPointer : Attachment
{

	[SerializeField] float range = 100f;
	[SerializeField] GameObject laser;
	[SerializeField] Color laserColor = Color.red;

	bool isOn = true;

	// Start is called before the first frame update
	void Start()
	{
		laser.GetComponent<MeshRenderer>().material.color = laserColor;

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			SwitchLaser();
		}

		if (isOn)
		{
			DrawLaser();
		}

	}

	void DrawLaser()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 100, 0, QueryTriggerInteraction.Ignore))
		{
			laser.SetActive(true);
			laser.transform.position = hit.normal;
		}
		else
		{
			laser.SetActive(false);
		}

	}

	void SwitchLaser()
	{
		isOn = !isOn;
	}
}
