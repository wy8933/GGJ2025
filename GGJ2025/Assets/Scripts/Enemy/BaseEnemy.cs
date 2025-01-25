using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemy : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;

    [Header("Base Stats")]
    public float maxHealth;
    public float health;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        EnemyPathFinding();
    }

    protected void EnemyPathFinding() {
        if (agent != null)
        {
            agent.destination = player.position;
        }
    }
}
