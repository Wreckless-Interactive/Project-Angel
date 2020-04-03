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

    private Vector3 startPos;

    private enum AIState { Chasing, Idle, Returning};
    private AIState currentState;

    public BattleCharacter battleCharacter;
    private EnemyGroup parentGroup;

    public void InitCharacter(Transform target, EnemyGroup group)
    {
        startPos = transform.position;
        currentState = AIState.Idle;
        this.target = target;
        parentGroup = group;
    }

    private void Update()
    {
        AILogic();
    }

    private void AILogic()
    {

        float distFromTarget = Vector3.Distance(transform.position, target.position);

        if (distFromTarget <= 10)
        {
            if (parentGroup.CanChase)
            {
                currentState = AIState.Chasing;
                if (distFromTarget <= 2)
                    parentGroup.StartBattle();
            }
            else
                currentState = AIState.Returning;

        }
        else if (distFromTarget > 10)
            currentState = AIState.Returning;

        switch (currentState)
        {
            case AIState.Idle:
                Agent.isStopped = true;
                break;
            case AIState.Chasing:
                Agent.speed = chaseSpeed;
                Agent.destination = target.position;
                Agent.isStopped = false;
                break;
            case AIState.Returning:
                Agent.speed = returnSpeed;
                Agent.destination = startPos;
                Agent.isStopped = false;
                break;
            default:
                Debug.LogError("currentState is equal to a nonexistant variable", this);
                break;
        }
    }


}
