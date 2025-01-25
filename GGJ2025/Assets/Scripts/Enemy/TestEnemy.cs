using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : BaseEnemy
{
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;
    public DamageType attackDamageType = DamageType.None;
    private bool canAttack = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        TryAttack();
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

        if (player.TryGetComponent(out PlayerController playerController))
        {
            DamageInfo damageInfo = new DamageInfo(gameObject, player.gameObject, attackDamage, attackDamageType);
            DamageManager.Instance.ManageDamage(damageInfo);
        }

        Invoke(nameof(ResetAttack), attackCooldown);
    }


    private void ResetAttack()
    {
        canAttack = true;
    }
}
