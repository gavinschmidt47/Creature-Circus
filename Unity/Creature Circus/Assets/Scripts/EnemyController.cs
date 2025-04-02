using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //Enemy movement
    [Header("Enemy Movement")]
    public Vector3 patrol1;
    public Vector3 patrol2;
    public Vector3 patrol3;
    public Vector3 patrol4;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 1;
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
        agent.SetDestination(patrol1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, patrol1) <= 30.5f && currentPatrolIndex == 1)
        {
            agent.SetDestination(patrol2);
            currentPatrolIndex = 2;
        }
        else if (Vector3.Distance(this.transform.position, patrol2) <= 30.5f && currentPatrolIndex == 2)
        {
            agent.SetDestination(patrol3);
            currentPatrolIndex = 3;
        }
        else if (Vector3.Distance(this.transform.position, patrol3) <= 30.5f && currentPatrolIndex == 3)
        {
            agent.SetDestination(patrol4);
            currentPatrolIndex = 4;
        }
        else if (Vector3.Distance(this.transform.position, patrol4) <= 30.5f && currentPatrolIndex == 4)
        {
            agent.SetDestination(patrol1);
            currentPatrolIndex = 1;
        }
    }
}
