using ObjectPoolings;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : BasicEnemy
{
    [Header("Boss Stats Mult")]
    public float healthMult;
    public float speedMult;

    public void InitEnemy(PrefabPool pool)
    {
        this.pool = pool;

        Stats.Health *= healthMult;
        Stats.MovementSpeed *= speedMult;

        Stats.Health = Stats.MaxHealth;
        agent.speed = Stats.MovementSpeed;
    }

    private void Update()
    {
        EnemyPathFinding();
        TryAttack();
    }

    // Boss Enemy only follow player
    protected override void EnemyPathFinding() 
    {
        if (agent != null)
        {
            agent.destination = player.position;
        }
    }
}
