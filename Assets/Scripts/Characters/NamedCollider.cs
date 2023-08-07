using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedCollider : MonoBehaviour
{
	public Collider collider;
	[SerializeField] string colliderName;

	public string ColliderName { get => colliderName; set => colliderName = value; }

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
