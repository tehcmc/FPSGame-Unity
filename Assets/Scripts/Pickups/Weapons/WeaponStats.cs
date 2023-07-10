using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class WeaponStats : MonoBehaviour
{

	[SerializeField] float verticalRecoil = 0.01f;
	[SerializeField] float horizontalRecoil = 0.01f;
	[SerializeField] float baseDamage = 1f;
	[SerializeField] float damageMultiplier = 1.2f;
	[SerializeField] float fireRate = 1f;

	public float VerticalRecoil { get => verticalRecoil; set => verticalRecoil = value; }
	public float HorizontalRecoil { get => horizontalRecoil; set => horizontalRecoil = value; }
	public float BaseDamage { get => baseDamage; set => baseDamage = value; }
	public float DamageMultiplier { get => damageMultiplier; set => damageMultiplier = value; }
	public float FireRate { get => fireRate; set => fireRate = value; }
}

