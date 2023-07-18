using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shotgun : RangedWeapon
{
	protected enum ShellType
	{
		Buckshot,
		Slug
	}

	[SerializeField] bool magFed = false;
	[SerializeField] protected ShellType type;
	[SerializeField] int pelletCount;


	protected override void Awake()
	{
		base.Awake();

	}
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

		if (!magFed)
		{

			weaponAnim.SetInteger("currentAmmo", currentAmmo);
			weaponAnim.SetInteger("clipSize", Mathf.RoundToInt(WeaponStats.GetStat(StatType.ClipSize)));
		}

	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		if (isReloading && Input.GetButtonDown("Fire1"))
		{
			if (magFed) return;
			if (weaponAnim) weaponAnim.SetTrigger("cancelReload");
		}

	}

	protected override void Fire()
	{
		isShooting = true;

		if (!suppressorAttached)
		{
			PlaySound("shoot");

		}
		else
		{
			PlaySound("suppressed");
		}


		weaponAnim.SetFloat("speedMult", Mathf.Clamp(weaponStats.GetStat(StatType.FireRate), 0, Mathf.Infinity));
		weaponAnim.SetTrigger("shoot");
		DrawMuzzleEffect();


		if (type == ShellType.Slug)
		{
			ShootBullet();
		}
		else
		{
			for (int i = 0; i < pelletCount; i++)
			{
				ShootBullet();
			}
		}



		currentAmmo--;
		GameManager.Instance.ChangeWeapnEvent();

		Recoil();
	}

	protected override void Reload()
	{
		if (weaponAnim)
		{

			isReloading = true;
			weaponAnim.SetBool("beginReload", true);
			weaponAnim.SetTrigger("reload");
			weaponAnim.SetInteger("currentAmmo", 0);
			weaponAnim.SetFloat("reloadSpeed", WeaponStats.GetStat(StatType.ReloadSpeed));

		}
		else
		{
			var ammoComp = player.GetComponent<Ammo>();
			if (!ammoComp) return;


			var clip = Mathf.RoundToInt(weaponStats.GetStat(StatType.ClipSize));

			var amount = ammoComp.TakeAmmo(weaponType, clip - currentAmmo);


			currentAmmo += amount;
			Debug.Log($"Current ammo (Reload): {currentAmmo}");
			GameManager.Instance.ChangeWeapnEvent();
		}


	}

	void LoadShell()
	{
		var ammoComp = player.GetComponent<Ammo>();
		if (!ammoComp) return;

		var clip = Mathf.RoundToInt(weaponStats.GetStat(StatType.ClipSize));
		var amount = ammoComp.TakeAmmo(weaponType, 1);


		currentAmmo += amount;
		GameManager.Instance.ChangeWeapnEvent();
		weaponAnim.SetInteger("currentAmmo", currentAmmo);

		int currentAmmoAnim = weaponAnim.GetInteger("currentAmmo");
		int clipSizeAnim = weaponAnim.GetInteger("clipSize");

		Debug.Log("anim clip " + clipSizeAnim);

		if (currentAmmoAnim >= clipSizeAnim || ammoComp.GetAmmo(weaponType) == 0)
		{
			weaponAnim.SetBool("beginReload", false);
			weaponAnim.SetTrigger("cancelReload");
		}



	}

	protected override void ReloadFinished()
	{
		isReloading = false;

	}
}
