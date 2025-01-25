using ObjectPoolings;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemy : MonoBehaviour
{
    public Stats Stats;
    public Transform player;
    public NavMeshAgent agent;
    public PrefabPool pool;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        EnemyPathFinding();
    }

    public void InitEnemy(PrefabPool pool) 
    {
        Stats.Health = Stats.MaxHealth;
        this.pool = pool;
    }

    protected void EnemyPathFinding() {
        if (agent != null)
        {
            agent.destination = player.position;
        }
    }

    public void TakeDamage(float damage) {
        Stats.Health -= damage;

        if (Stats.Health <= 0) {
            Die();
        }
    }

    private void Die() 
    {
        pool.Release(gameObject);
    }
}
