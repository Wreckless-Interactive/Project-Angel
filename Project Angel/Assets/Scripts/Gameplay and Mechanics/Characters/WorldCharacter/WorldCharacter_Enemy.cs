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


}
