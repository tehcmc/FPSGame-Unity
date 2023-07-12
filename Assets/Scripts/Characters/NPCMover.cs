using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

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

	Transform target;

	[SerializeField] bool showDebugLogs = false;
	[SerializeField] float chaseRange = 5f;
	[SerializeField] float searchRadius = 10f;
	[SerializeField] float turnSpeed = 5f;
	Animator animator;




	SphereCollider searchCollider;
	NavMeshAgent agent;
	float distanceToTarget = Mathf.Infinity;
	Vector3 startPos;
	State state = State.Idle;

	public Transform Target { get => target; set => target = value; }

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
		if (showDebugLogs) Debug.Log("NEW TARGET");
		Target = targetCharacter.transform;
		state = State.Chase;

	}
	void Update()
	{
		if (showDebugLogs) Debug.Log(state);

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

		if (!Target || !Target.GetComponent<Character>()) { state = State.ReturntoStart; return; }



		distanceToTarget = Vector3.Distance(Target.position, transform.position);


		Vector3 dir = (transform.position - target.position).normalized;
		Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));

		transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * turnSpeed);


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

		}
		else
		{
			state = State.Idle;
		}

	}
	void ChaseTarget()
	{
		if (animator) animator.SetTrigger("move");
		agent.SetDestination(Target.position);
		distanceToTarget = Vector3.Distance(Target.position, transform.position);

		if (distanceToTarget <= agent.stoppingDistance)
		{
			state = State.Attack;
		}


		if (distanceToTarget >= chaseRange)
		{
			state = State.ReturntoStart;
			Target = null;
		}
	}
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, chaseRange);
	}


	public void FindNearestCharacter()
	{
		Transform characters = FindObjectOfType<Player>().transform;
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach (Transform potentialTarget in characters)
		{
			Vector3 directionToTarget = potentialTarget.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}

		state = State.Chase;
		target = bestTarget;


	}
}
