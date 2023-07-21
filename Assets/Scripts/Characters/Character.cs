using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class DamagePoint
{

	[SerializeField] string name;
	[SerializeField] Transform point;
	[SerializeField] float damageMultiplier;



	public string Name { get => name; set => name = value; }
	public Transform Point { get => point; set => point = value; }
	public float DamageMultiplier { get => damageMultiplier; set => damageMultiplier = value; }

}




[RequireComponent(typeof(Health))]
public class Character : MonoBehaviour
{
	Health health;


	// positional damage - based on area hit, get hit collider, get damage multiplier from that collider (dictionary?) and apply bullet damage + multiplier to character. return damage to bullet class to apply to popup???
	[SerializeField] List<DamagePoint> points;

	IDictionary<Transform, DamagePoint> damagePoints = new Dictionary<Transform, DamagePoint>();



	// Start is called before the first frame update
	protected virtual void Awake()
	{

		health = GetComponent<Health>();
		foreach (DamagePoint point in points)
		{
			damagePoints.Add(point.Point, point);
		}
	}
	private void OnEnable()
	{

		health.OnDamage += TakeDamage;
		health.OnZeroed += Die;
	}
	private void OnDisable()
	{
		health.OnDamage -= TakeDamage;
		health.OnZeroed -= Die;
	}

	void Start()
	{

	}

	void Update()
	{

	}

	protected virtual void TakeDamage()
	{
		Debug.Log(gameObject.name + ": Ouch");
	}
	protected virtual void Die()
	{
		//base class does nothing!
	}

	public DamagePoint GetDamagePoint(Transform transform)
	{
		if (!damagePoints.ContainsKey(transform)) return null;

		return damagePoints[transform];

	}

	public float GetDamageMultiplier(Transform transform)
	{
		if (!damagePoints.ContainsKey(transform)) return 1;

		return damagePoints[transform].DamageMultiplier;
	}


}
