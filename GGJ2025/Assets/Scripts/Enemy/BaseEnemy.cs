using ObjectPoolings;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemy : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public PrefabPool pool;

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

    public void InitEnemy(PrefabPool pool) 
    {
        health = maxHealth;
        this.pool = pool;
    }

    protected void EnemyPathFinding() {
        if (agent != null)
        {
            agent.destination = player.position;
        }
    }

    public void TakeDamage(float damage) { 
        health -= damage;

        if (health <= 0) {
            Die();
        }
    }

    private void Die() 
    {
        pool.Release(gameObject);
    }
}
