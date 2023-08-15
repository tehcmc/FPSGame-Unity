using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class ThrowableWeapon : Weapon
{
	bool isThrowwing = false;
	bool throwPrep = false;
	[SerializeField] Grenade grenade;
	[SerializeField] float powerLimit = 500f;
	[SerializeField] float powerIncreaseRate = 300f;


	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] Transform releasePosition;
	[SerializeField] int startCount = 1;
	[SerializeField] int pickupCount = 3;
	float throwPower = 100f;
	float defaultPower;
	float mass;

	public int currentCount { get; protected set; }


	[SerializeField][Range(10, 100)] int linePoints = 25;

	[SerializeField][Range(0.01f, 0.25f)] float timeBetweenPoints = 0.1f;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		releasePosition = gameObject.transform;
		defaultPower = throwPower;
		currentCount = startCount;


		base.Awake();


		if (!grenade) Destroy(gameObject);

		var nade = Instantiate(grenade);
		mass = nade.Rb.mass;
		Destroy(nade.gameObject);
	}

	void Update()
	{
		weaponAnim.SetBool("prepThrow", PreparedToThrow());

		if (CanThrow())
		{
			BeginThrow();
		}

		if (PreparedToThrow())
		{
			DrawProjection();
			if (Input.GetButton("Fire1"))
			{

				ChargeThrow();
			}
		}
		else
		{
			lineRenderer.enabled = false;
			throwPower = defaultPower;
			Debug.Log(throwPower);
		}
	}

	bool CanThrow()
	{
		if (isThrowwing) return false;
		if (!PreparedToThrow()) return false;
		if (!Input.GetButtonUp("Fire1")) return false;
		return true;
	}

	void ChargeThrow()
	{
		throwPower += powerIncreaseRate * Time.deltaTime;
		throwPower = Mathf.Clamp(throwPower, 0, powerLimit);
		Debug.Log(throwPower);
	}

	bool PreparedToThrow()
	{
		if (isThrowwing) return false;
		if (!Input.GetButton("Fire2")) return false;


		return true;
	}

	void DrawProjection()
	{
		lineRenderer.enabled = true;
		lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;



		Vector3 startPosition = releasePosition.position;


		Vector3 startVelocity = throwPower * cam.transform.forward / 1.2f;// / mass;
		int i = 0;
		lineRenderer.SetPosition(i, startPosition);


		for (float time = 0; time < linePoints; time += timeBetweenPoints)
		{
			i++;
			Vector3 point = startPosition + time * startVelocity;


			point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

			lineRenderer.SetPosition(i, point);
		}
	}

	void BeginThrow()
	{
		weaponAnim.SetTrigger("throw");

	}

	void Throw()
	{
		var obj = Instantiate(grenade, transform.position, Quaternion.identity);
		obj.ShootProjectile(transform.forward * throwPower);
		currentCount--;

		if (currentCount <= 0)
		{
			gameObject.SetActive(false);
			GameManager.Instance.ChangeWeapnEvent();
			Destroy(gameObject);
		}

		GameManager.Instance.ChangeWeapnEvent();
		throwPower = defaultPower;
	}

	void EndThrow()
	{


		isThrowwing = false;

	}

	public override bool CanPickUp(string name)
	{

		if (name == null) return false;


		if (name == objectName)
		{

			currentCount += pickupCount;
			GameManager.Instance.ChangeWeapnEvent();
			return false;
		}
		return true;
	}
}
