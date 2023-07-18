using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
	bool isAttacking = false;

	public bool IsAttacking { get => isAttacking; set => isAttacking = value; }


	// Start is called before the first frame update
	void Start()
	{

	}


	protected bool CanAttack()
	{
		if (IsAttacking) return false;
		if (!Input.GetButtonDown("Fire1")) return false;
		return true;
	}
	// Update is called once per frame
	void Update()
	{
		if (CanAttack())
		{
			Attack();
		}
	}

	void Attack()
	{
		IsAttacking = true;
		weaponAnim.SetTrigger("attack");
	}

	void FinishedAttack()
	{
		IsAttacking = false;
	}
}
