using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
	public static GameEvents instance;


	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);

		}
		else
		{
			instance = this;
		}
	}

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
