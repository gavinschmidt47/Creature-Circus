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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (agent.destination == patrol1)
            {
            agent.SetDestination(patrol2);
            }
            else if (agent.destination == patrol2)
            {
            agent.SetDestination(patrol3);
            }
            else if (agent.destination == patrol3)
            {
            agent.SetDestination(patrol4);
            }
            else
            {
            agent.SetDestination(patrol1);
            }
        }
    }
}
