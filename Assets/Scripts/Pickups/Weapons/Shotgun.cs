using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shotgun : Weapon
{
	protected enum ShellType
	{
		Buckshot,
		Slug
	}

	[SerializeField] protected ShellType type;
	[SerializeField] int pelletCount;

	protected override void Awake()
	{
		base.Awake();

	}
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

	}

	protected override void Fire()
	{
		if (type == ShellType.Slug)
		{
			base.Fire();
			return;
		}


		//shotgun logic. same as fire except instead of 1 raycast, do x amount (based on pelletCount). use vertical and horizontal recoil??

		for (int i = 0; i < pelletCount; i++)
		{
			base.Fire();



		}




	}
}
