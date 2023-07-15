using UnityEngine;

public class AmmoBox : MonoBehaviour
{
	[SerializeField] WeaponType ammoType;
	[SerializeField] int ammoCount = 100;
	[SerializeField] bool reusable = false;


	[Header("Ammo Type Icons")]
	[SerializeField] GameObject pistolIcon;
	[SerializeField] GameObject smgIcon;
	[SerializeField] GameObject rifleIcon;
	[SerializeField] GameObject shotgunIcon;
	[SerializeField] GameObject sniperIcon;
	[SerializeField] GameObject specialIcon;

	private void Awake()
	{
		HideAllIcons();
		switch (ammoType)
		{
			case WeaponType.Pistol:
				pistolIcon.SetActive(true);
				break;
			case WeaponType.SMG:
				smgIcon.SetActive(true);
				break;
			case WeaponType.Rifle:
				rifleIcon.SetActive(true);
				break;
			case WeaponType.Shotgun:
				shotgunIcon.SetActive(true);
				break;
			case WeaponType.Sniper:
				sniperIcon.SetActive(true);
				break;
			case WeaponType.Special:
				specialIcon.SetActive(true);
				break;
		}



	}

	private void OnTriggerEnter(Collider other)
	{
		var ammo = other.gameObject.GetComponent<Ammo>();

		if (!ammo) return;

		ammo.GiveAmmo(ammoType, ammoCount);

		if (!reusable)
		{
			Destroy(gameObject);
		}
	}
	void HideAllIcons()
	{
		pistolIcon.SetActive(false);
		smgIcon.SetActive(false);
		rifleIcon.SetActive(false);
		shotgunIcon.SetActive(false);
		sniperIcon.SetActive(false);
		specialIcon.SetActive(false);
	}
}
