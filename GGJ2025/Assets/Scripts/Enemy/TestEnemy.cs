using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;

    [Header("Attack Settings")]
    public float attackRange = 2f; // Distance at which the enemy attacks
    public float attackCooldown = 1.5f; // Time between attacks
    public int attackDamage = 10; // Damage dealt per attack

    private bool canAttack = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        EnemyPathFinding();
        TryAttack();
    }

    protected void EnemyPathFinding()
    {
        if (agent != null)
        {
            agent.destination = player.position;
        }
    }

    private void TryAttack()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false;

        Debug.Log("Enemy attacks!");
        


        Invoke(nameof(ResetAttack), attackCooldown);
    }


    private void ResetAttack()
    {
        canAttack = true;
    }
}
