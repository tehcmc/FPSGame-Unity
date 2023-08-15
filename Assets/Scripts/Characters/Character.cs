using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class DamagePoint
{

	[SerializeField] string name;
	[SerializeField] float damageMultiplier;



	public string Name { get => name; set => name = value; }
	public float DamageMultiplier { get => damageMultiplier; set => damageMultiplier = value; }

}




[RequireComponent(typeof(Health))]
public class Character : MonoBehaviour
{
	protected Health health;
	public bool IsAlive { get; protected set; } = true;

	// positional damage - based on area hit, get hit collider, get damage multiplier from that collider (dictionary?) and apply bullet damage + multiplier to character. return damage to bullet class to apply to popup???
	[SerializeField] List<DamagePoint> damagePoints;

	IDictionary<string, DamagePoint> damagePointDict = new Dictionary<string, DamagePoint>();



	// Start is called before the first frame update
	protected virtual void Awake()
	{

		health = GetComponent<Health>();
		foreach (DamagePoint point in damagePoints)
		{
			damagePointDict.Add(point.Name, point);
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
		Debug.Log(health.currentHealth);
	}

	protected virtual void Die()
	{
		IsAlive = false;
		//base class does nothing!
	}

	public DamagePoint GetDamagePoint(string name)
	{
		if (!damagePointDict.ContainsKey(name)) return null;
		Debug.Log($"Point: {damagePointDict[name]}");
		return damagePointDict[name];

	}

	public float GetDamageMultiplier(string name)
	{
		if (!damagePointDict.ContainsKey(name)) return 1;

		return damagePointDict[name].DamageMultiplier;
	}


}
