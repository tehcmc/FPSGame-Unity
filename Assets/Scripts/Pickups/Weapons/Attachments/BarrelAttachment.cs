using UnityEngine;

public class BarrelAttachment : Attachment
{
	[SerializeField] ParticleSystem muzzleEffect;
	[SerializeField] AudioClip shootSound;

	protected override void OnEnable()
	{
		base.OnEnable();
		if (muzzleEffect) myWeapon.MuzzleFlash = muzzleEffect;
		if (shootSound) myWeapon.FireSound = shootSound;
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		if (muzzleEffect) myWeapon.MuzzleFlash = myWeapon.DefaultMuzzle;
		if (shootSound) myWeapon.FireSound = myWeapon.DefaultFireSound;
	}
	void Start()
	{

	}

	void Update()
	{

	}
}
