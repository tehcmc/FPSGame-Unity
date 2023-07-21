using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ProjectileShooterTest : MonoBehaviour
{
	[SerializeField] Projectile bullet;
	[SerializeField] Transform shootPos;
	Animator animator;
	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (bullet)
			{
				animator.SetTrigger("shoot");
			}
		}
	}

	void Shoot()
	{
		Instantiate(bullet, shootPos.transform.position, transform.rotation);
	}

}
