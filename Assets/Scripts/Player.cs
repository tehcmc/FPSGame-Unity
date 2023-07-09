using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(DeathHandler))]
public class Player : Character
{
	DeathHandler deathHandler;
	// Start is called before the first frame update

	protected virtual void Awake()
	{
		base.Awake();
		deathHandler = GetComponent<DeathHandler>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	protected override void Die()
	{
		deathHandler.ShowCanvas();
		gameObject.SetActive(false);
		base.Die();
	}
}
