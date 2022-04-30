using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Light that chases the player.
public class LightAttack : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    public float speed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}
