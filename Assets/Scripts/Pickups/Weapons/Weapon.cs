using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] Camera cam;
	[SerializeField] float bulletRange = 100f;
	[SerializeField] float bulletDamage = 10f;
	void Update()
	{
		if (CanFire())
		{
			Fire();
		}
	}

	void Fire()
	{
		RaycastHit tr;
		Debug.DrawLine(cam.transform.position, cam.transform.forward * bulletRange, Color.red);
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out tr, bulletRange))
		{
			if (tr.transform != transform.parent)
			{
				var health = tr.collider.gameObject.GetComponent<Health>();
				if (health)
				{
					health.ModifyHealth(-bulletDamage);
				}
				Debug.Log(tr.collider.name);
			}

		}



	}
	public bool CanFire()
	{
		if (!Input.GetButtonDown("Fire1")) return false;



		return true;

	}
}
