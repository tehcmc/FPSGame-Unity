using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
public enum StatType
{
	VerticalRecoil,
	HorizontalRecoil,
	FireRate,
	BaseDamage,
	DamageMultiplier,
	Range
}
public class WeaponStats : MonoBehaviour
{



	[SerializeField] List<Stat> stats = new();
	public List<Stat> Stats { get => stats; set => stats = value; }

	IDictionary<StatType, float> statDictionary = new Dictionary<StatType, float>();

	public IDictionary<StatType, float> StatDictionary { get => statDictionary; set => statDictionary = value; }


	private void OnEnable()
	{
		PopulateDictionary();
	}
	void PopulateDictionary()
	{
		foreach (var stat in stats)
		{
			if (!StatDictionary.ContainsKey(stat.StatType))
			{
				StatDictionary.Add(stat.StatType, stat.Value);
			}

		}
	}

	public void SetStat(StatType stat, float val)
	{
		if (!statDictionary.ContainsKey(stat)) return;
		statDictionary[stat] = val;


	}

	public void ModifyStat(StatType stat, float val)
	{

		if (!statDictionary.ContainsKey(stat)) { Debug.Log("INVALID KEY!"); return; }

		Debug.Log($"{statDictionary[stat]} += {val}");
		statDictionary[stat] += val;

		Debug.Log($"{statDictionary[stat]}");
	}
	public float GetStat(StatType stat)
	{
		if (!statDictionary.ContainsKey(stat)) return 0f;
		return statDictionary[stat];
	}

}

