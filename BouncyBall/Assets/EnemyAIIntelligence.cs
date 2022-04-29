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
    public FSMStates currentState;
    public float enemySpeed = 5f;

    GameObject[] wanderPoints;
    Vector3 nextDestination;
    float distanceToPlayer;
    float elapsedTime = 0f;

    int currentDestinationIndex = 0;

    public NavMeshAgent agent;

    public Transform enemyEyes;
    public float fieldOfView = 45f;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        player = GameObject.FindGameObjectWithTag("Player");
        Initialize();

        agent = GetComponent<NavMeshAgent>();
        isDead = false;
    }

    // Update is called once per frame
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

        elapsedTime += Time.deltaTime;
    }

    void Initialize()
    {
        currentState = FSMStates.Patrol;
        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        print("Patrolling!");

        agent.speed = 3.5f;

        agent.stoppingDistance = 0;

        if (Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }
        else if (IsPlayerInClearFOV())
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }

    void UpdateChaseState()
    {
        print("Chasing!");

        agent.stoppingDistance = chaseDistance;

        agent.speed = 5;

        nextDestination = player.transform.position;

        if (distanceToPlayer > chaseDistance)
        {
            FindNextPoint();
            currentState = FSMStates.Patrol;
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

    private void OnDrawGizmos()
    {
        // chase
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);

        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    print("Player in sight");
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }
}
