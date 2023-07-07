using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


enum States
{
	Idle,
	ReturntoStart,
	Chase,
	Attack,
	Die
}
public class NPCMover : MonoBehaviour
{

	[SerializeField] Transform target;
	[SerializeField] float chaseRange = 5f;
	[SerializeField] float attackRange = 5f;
	[SerializeField] float searchRadius = 10f;

	SphereCollider searchCollider;
	NavMeshAgent agent;
	float distanceToTarget = Mathf.Infinity;
	Vector3 startPos;
	States states;

	void Awake()
	{

		searchCollider = GetComponent<SphereCollider>();
		agent = GetComponent<NavMeshAgent>();

		if (searchCollider) searchCollider.radius = searchRadius;
	}

	void Start()
	{
		startPos = transform.position;
	}
	void Update()
	{
		Move();
		//states - idle - doing nothing
		// return to start - moving back to start pos
		// chase - chasing target
		// attack - attacking target
		// die - self explanitory

	}

	void Move()
	{

		if (!target)
		{
			if (Vector3.Distance(startPos, transform.position) >= agent.stoppingDistance)
			{
				Debug.Log("Returning to start"); agent.SetDestination(startPos);
			}
			return;
		}




		agent.SetDestination(target.position);
		distanceToTarget = Vector3.Distance(target.position, transform.position);
		if (distanceToTarget <= agent.stoppingDistance)
		{
			Attack();
		}
		if (distanceToTarget >= chaseRange)
		{
			Debug.Log("TARGET LOST");
			target = null;
		}

	}
	void Attack()
	{
		if (!target) return;
		if (!target.GetComponent<Character>()) return;

		Debug.Log("Attack");

	}
	private void OnTriggerEnter(Collider other)
	{

		var targetCharacter = other.GetComponent<Character>();

		if (!targetCharacter) return;


		Debug.Log("NEW TARGET");
		target = targetCharacter.transform;

	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, chaseRange);
	}


}
