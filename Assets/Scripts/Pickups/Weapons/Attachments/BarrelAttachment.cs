using UnityEngine;

public class BarrelAttachment : Attachment
{
	[SerializeField] ParticleSystem muzzleEffect;
	[SerializeField] AudioClip shootSound;

	[SerializeField] Transform shootPosition;
	[SerializeField] bool suppressor = false;

	protected override void OnEnable()
	{
		base.OnEnable();
		if (muzzleEffect) myWeapon.MuzzleFlash = muzzleEffect;
		if (shootPosition) myWeapon.ShootPosition = shootPosition;
		if (suppressor) myWeapon.SuppressorAttached = true;
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		if (muzzleEffect) myWeapon.MuzzleFlash = myWeapon.DefaultMuzzle;
		if (shootPosition) myWeapon.ShootPosition = myWeapon.DefaultMuzzlePosition;
		if (suppressor) myWeapon.SuppressorAttached = false;


	}

}
