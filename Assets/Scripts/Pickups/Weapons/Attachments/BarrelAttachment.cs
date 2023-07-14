using UnityEngine;

public class BarrelAttachment : Attachment
{
	[SerializeField] ParticleSystem muzzleEffect;
	[SerializeField] AudioClip shootSound;

	protected override void OnEnable()
	{
		base.OnEnable();
		if (muzzleEffect) myWeapon.MuzzleFlash = muzzleEffect;

	}
	protected override void OnDisable()
	{
		base.OnDisable();
	}
	void Start()
	{

	}

	void Update()
	{

	}
}
