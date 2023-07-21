using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponStats))]
public class MeleeWeapon : Weapon
{
	bool isAttacking = false;
	WeaponStats weaponStats;
	[SerializeField] ParticleSystem hitParticle;
	public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

	protected override void Awake()
	{
		base.Awake();
		weaponStats = GetComponent<WeaponStats>();
	}
	protected bool CanAttack()
	{

		if (IsAttacking) return false;
		if (!Input.GetButtonDown("Fire1")) return false;

		return true;

	}

	void Update()
	{

		if (CanAttack())
		{
			Attack();
		}

	}

	void Attack()
	{
		isAttacking = true;
		Debug.Log("attack");
		weaponAnim.SetTrigger("attack");

	}
	protected void DoHit()
	{

		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit tr, Mathf.Clamp(weaponStats.GetStat(StatType.Range), 0, Mathf.Infinity), 99, QueryTriggerInteraction.Ignore))
		{
			if (tr.point == null) return;
			if (tr.transform == transform.parent) return;
			Debug.Log("hit");
			DamageCharacter(tr);
			DoImpactEffect(tr);
		}

	}

	protected void DamageCharacter(RaycastHit tr)
	{

		var character = tr.collider.gameObject.transform.parent.GetComponentInChildren<Character>();
		if (!character) return;
		Debug.Log("hit character");
		var health = character.GetComponent<Health>();
		float mult = character.GetDamageMultiplier(tr.collider.transform);

		DamagePoint damagePoint = character.GetDamagePoint(tr.collider.transform);

		if (damagePoint != null && (damagePoint.Name.ToUpper() == "HEAD"))
		{
			player.AudioSource.PlayOneShot(GameManager.Instance.HeadshotSound);

		}


		float damage = (weaponStats.GetStat(StatType.BaseDamage) * weaponStats.GetStat(StatType.DamageMultiplier)) * mult;
		Debug.Log("hit character 2");
		health.DamageHealth(damage);
		Debug.Log("hit character 3");
		ShowDamage(damage, tr.point);

	}
	protected void DoImpactEffect(RaycastHit tr)
	{
		Instantiate(hitParticle, tr.point, Quaternion.LookRotation(tr.normal));
	}

	void AnimTest()
	{
		Debug.Log("play anim");
	}

	void FinishedAttack()
	{
		IsAttacking = false;
	}
}
