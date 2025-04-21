using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //Enemy movement
    [Header("Enemy Movement")]
    public Vector3[] patrolPoints;
    [Tooltip("Distance to change patrol points")]
    public float patrolChange = 10f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
            return;
        }
        // Set the initial destination to patrol1
        agent.SetDestination(patrolPoints[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, patrolPoints[currentPatrolIndex]) <= patrolChange)
        {
            // Move to the next patrol point
            if (currentPatrolIndex + 1 < patrolPoints.Length)
            {
                currentPatrolIndex++;
            }
            else
            {
                currentPatrolIndex = 0; // Loop back to the first point
            }
            // Set the new destination
            agent.SetDestination(patrolPoints[currentPatrolIndex]);
        }
    }
}
