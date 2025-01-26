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
    public bool isReleased;

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
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Stats.MovementSpeed;
        isReleased = false;
    }

    protected virtual void EnemyPathFinding()
    {
        Vector3 woundPosition = Wound.Instance.transform.position;
        Vector3 targetPosition = woundPosition;

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float distanceToWound = Vector3.Distance(transform.position, woundPosition);

            // If the player is closer than the wound, target the player instead
            if (distanceToPlayer <= distanceToWound)
            {
                targetPosition = player.position;
            }
        }

        agent.SetDestination(targetPosition);
        
    }

    public void TakeDamage(float damage) {
        Stats.Health -= damage;

        if (Stats.Health <= 0) {
            Die();
        }
    }

    private void Die() 
    {
        if (!isReleased) {
            isReleased = true;
            EnemyWaveManager.Instance.EnemyDefeated();
            pool.Release(gameObject);
        }
    }

    private void BubbleDeathAnimation() { 
        
    }
    private void OnDrawGizmos()
    {
        if (agent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, agent.destination);
        }
    }

}
