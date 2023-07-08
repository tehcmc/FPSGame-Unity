using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


enum State
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
	[SerializeField] float searchRadius = 10f;
	Animator animator;




	SphereCollider searchCollider;
	NavMeshAgent agent;
	float distanceToTarget = Mathf.Infinity;
	Vector3 startPos;
	State state = State.Idle;

	void Awake()
	{

		searchCollider = GetComponent<SphereCollider>();
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();

		if (searchCollider) searchCollider.radius = searchRadius;
	}

	void Start()
	{
		startPos = transform.position;
	}
	private void OnTriggerEnter(Collider other)
	{

		var targetCharacter = other.GetComponent<Character>();
		if (!targetCharacter) return;
		Debug.Log("NEW TARGET");
		target = targetCharacter.transform;
		state = State.Chase;

	}
	void Update()
	{
		Debug.Log(state);
		switch (state)
		{
			case State.Idle:
				Idle();
				break;
			case State.ReturntoStart:
				ReturnToStart();
				break;
			case State.Chase:
				ChaseTarget();
				break;
			case State.Attack:
				Attack();
				break;
			default:
				Idle();
				break;



		}
		//states - idle - doing nothing
		// return to start - moving back to start pos
		// chase - chasing target
		// attack - attacking target
		// die - self explanitory

	}

	void Attack()
	{
		if (animator) animator.SetBool("attack", true);

		if (!target || !target.GetComponent<Character>()) state = State.ReturntoStart;

		distanceToTarget = Vector3.Distance(target.position, transform.position);

		if (distanceToTarget > agent.stoppingDistance)
		{
			if (animator) animator.SetBool("attack", false);
			state = State.Chase;
		}

	}

	void Idle()
	{
		if (animator) animator.SetTrigger("idle");
	}

	void ReturnToStart()
	{
		if (animator) animator.SetTrigger("move");
		if (Vector3.Distance(startPos, transform.position) > agent.stoppingDistance)
		{
			Debug.Log("Returning to start"); agent.SetDestination(startPos);
		}
		else
		{
			state = State.Idle;
		}

	}
	void ChaseTarget()
	{
		if (animator) animator.SetTrigger("move");
		agent.SetDestination(target.position);
		distanceToTarget = Vector3.Distance(target.position, transform.position);

		if (distanceToTarget <= agent.stoppingDistance)
		{
			state = State.Attack;
		}


		if (distanceToTarget >= chaseRange)
		{
			state = State.ReturntoStart;
			target = null;
		}
	}
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, chaseRange);
	}


}
