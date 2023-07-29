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
	float attackRange;
	Vector3 startPos;
	State state = State.Idle;

	public Transform Target { get => target; set => target = value; }
	public float AttackRange { get => attackRange; set => attackRange = value; }
	public Animator Animator { get => animator; set => animator = value; }

	void Awake()
	{

		searchCollider = GetComponent<SphereCollider>();
		Animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		AttackRange = agent.stoppingDistance;
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
	}

	void Attack()
	{



		agent.SetDestination(transform.position);

		if (!Target || !Target.GetComponent<Character>()) { state = State.ReturntoStart; return; }
		distanceToTarget = Vector3.Distance(Target.position, transform.position);
		Vector3 dir = (transform.position - target.position).normalized;
		Quaternion lookRot = Quaternion.LookRotation(new Vector3(-dir.x, 0, -dir.z));

		transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * turnSpeed);
		if (Animator) Animator.SetBool("attack", true);


		if (distanceToTarget > AttackRange)
		{
			if (Animator) Animator.SetBool("attack", false);
			state = State.Chase;
		}

	}

	void Idle()
	{
		if (Animator) Animator.SetTrigger("idle");
	}

	void ReturnToStart()
	{
		if (Animator) Animator.SetTrigger("move");

		if (Vector3.Distance(startPos, transform.position) < agent.stoppingDistance)
		{
			state = State.Idle;
		}

	}
	void ChaseTarget()
	{
		if (Animator) Animator.SetTrigger("move");
		agent.SetDestination(Target.position);
		distanceToTarget = Vector3.Distance(Target.position, transform.position);

		if (distanceToTarget <= AttackRange)
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
		Transform characters = FindObjectOfType<Player>().transform; //for now, find player. If expanding to multiple players, and/or factions, modify this.
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
