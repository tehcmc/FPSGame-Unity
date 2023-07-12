using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
	[SerializeField] StatType statType;
	[SerializeField] float value;
	// Start is called before the first frame update

	public Stat(StatType statType, float val)
	{
		this.statType = statType;
		this.value = val;
	}

	public float Value { get => value; set => this.value = value; }
	public StatType StatType { get => statType; set => statType = value; }
}
