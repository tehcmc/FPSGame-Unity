using UnityEngine;

public class BarrelAttachment : Attachment
{
	[SerializeField] ParticleSystem muzzleEffect;
	[SerializeField] AudioClip shootSound;

	[SerializeField] Transform shootPosition;

	protected override void OnEnable()
	{
		base.OnEnable();
		if (muzzleEffect) myWeapon.MuzzleFlash = muzzleEffect;
		if (shootSound) myWeapon.FireSound = shootSound;
		if (shootPosition) myWeapon.ShootPosition = shootPosition;
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		if (muzzleEffect) myWeapon.MuzzleFlash = myWeapon.DefaultMuzzle;
		if (shootSound) myWeapon.FireSound = myWeapon.DefaultFireSound;
		if (shootPosition) myWeapon.ShootPosition = myWeapon.DefaultMuzzlePosition;
	}

}
