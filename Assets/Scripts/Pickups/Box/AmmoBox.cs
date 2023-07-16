using UnityEngine;

public class AmmoBox : MonoBehaviour
{
	[SerializeField] WeaponType ammoType;
	[SerializeField] int ammoCount = 100;
	[SerializeField] bool reusable = false;




	private void OnTriggerEnter(Collider other)
	{
		var ammo = other.gameObject.GetComponent<Ammo>();

		if (!ammo) return;

		ammo.GiveAmmo(ammoType, ammoCount);

		ShowAmmo(ammoCount, gameObject.transform.position);

		if (!reusable)
		{
			Destroy(gameObject);
		}
	}
	void ShowAmmo(float val, Vector3 spawnPoint)
	{

		Camera camera = Camera.main;
		if (!camera) return;
		var popup = Instantiate(GameManager.Instance.DmgPopup, spawnPoint, Quaternion.LookRotation(spawnPoint - camera.transform.position));
		popup.DisplayDamage(val);

	}


}
