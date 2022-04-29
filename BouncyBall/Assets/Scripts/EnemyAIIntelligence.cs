using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIIntelligence : MonoBehaviour
{
    public enum FSMStates
    {
        Patrol,
        Chase,
    }

    public float chaseDistance = 10f;
    public GameObject player;
    public FSMStates currentState = FSMStates.Patrol;
    public float enemySpeedChase = 10f;
    public float enemySpeedPatrol = 5f;
    public float minDistanceToWanderPoint = 10f;
    public Color patrolLightColor;
    public Color chaseLightColor;
    public Light spotlight; // Spotlight of view

    public GameObject[] wanderPoints; // To be selected through editor (unique to each enemy)
    Vector3 nextDestination;
    float distanceToPlayer;
    AudioSource myAlarm; // To be sounded when chasing player

    int currentDestinationIndex = 0;

    public NavMeshAgent agent;
    public Transform enemyEyes;
    public float fieldOfView = 45f;

    void Start()
    {
        myAlarm = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        Initialize();

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
        }

        Debug.Log(nextDestination);
    }

    void Initialize()
    {
        currentState = FSMStates.Patrol;
        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        spotlight.color = patrolLightColor;
        agent.speed = enemySpeedPatrol;
        agent.stoppingDistance = 0;

        if (Vector3.Distance(transform.position, nextDestination) < minDistanceToWanderPoint)
        {
            FindNextPoint();
        }
        
        if (IsPlayerInClearFOV())
        {
            currentState = FSMStates.Chase;
            myAlarm.Play();
        }

        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }

    void UpdateChaseState()
    {
        spotlight.color = chaseLightColor;
        agent.stoppingDistance = 0;
        agent.speed = enemySpeedChase;
        nextDestination = player.transform.position;

        if (distanceToPlayer > chaseDistance)
        {
            FindNextPoint();
            currentState = FSMStates.Patrol;
            myAlarm.Stop();
        }

        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
        agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }
    
    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;
        
        return Vector3.Angle(directionToPlayer, transform.forward) <= fieldOfView
            && Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance)
            && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Crush"));
    }
}
