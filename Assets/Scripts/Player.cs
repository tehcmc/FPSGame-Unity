using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public override void Die()
	{

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		base.Die();
	}
}
