using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WorldCharacter_Enemy : WorldCharacter
{

    private NavMeshAgent Agent => GetComponent<NavMeshAgent>();

    public float chaseSpeed;
    public float returnSpeed;

    private Transform target;

    public bool chaseTarget;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {

        if (!chaseTarget && target == null)
            return;

        print("T");

        Agent.destination = target.position;

    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        chaseTarget = true;
        Agent.speed = chaseSpeed;
    }

    public void ReturnToPoint()
    {
        chaseTarget = false;
        Agent.destination = startPos;
        Agent.speed = returnSpeed;
    }


}
