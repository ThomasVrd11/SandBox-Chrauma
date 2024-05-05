using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class enemyBehavior : MonoBehaviour
{
	GameObject target;
	[SerializeField] Transform RayOrigin;
	NavMeshAgent navMeshAgent;
	RaycastHit hit;
	Animator mobAnim;
	int isWalkingHash;
	int isRunningHash;

    private void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent>();
		mobAnim = GetComponent<Animator>();
		isWalkingHash = Animator.StringToHash("isWalking");
		isRunningHash = Animator.StringToHash("isRunning");
		target = GameObject.FindGameObjectWithTag("Player");
	}


    // Update is called once per frame
    void Update()
    {
		var distance = Vector3.Distance(transform.position, target.transform.position);
		Vector3 direction = (target.transform.position - RayOrigin.position).normalized;

		direction.y = 0;  // Make the ray horizontal if vertical component is not critical

		if (distance < 10.0f)
		{
			Debug.DrawRay(RayOrigin.position, direction * 100, Color.green);
			if (Physics.Raycast(RayOrigin.position, direction, out hit))
			{
				if (hit.transform.name == "Player")
				{
					navMeshAgent.SetDestination(target.transform.position);
					mobAnim.SetBool(isWalkingHash, true);
					mobAnim.SetBool(isRunningHash, true);
					
				}
			}
		} else
		{
			navMeshAgent.SetDestination(transform.position);
			mobAnim.SetBool(isWalkingHash, false);
			mobAnim.SetBool(isRunningHash, false);
		}
    }

	void handleAnimation()
	{
		bool isWalking = mobAnim.GetBool(isWalkingHash);
		bool isRunning = mobAnim.GetBool(isRunningHash);
	}
}
