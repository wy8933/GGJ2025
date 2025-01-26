using ObjectPoolings;
using UnityEngine;

public class BossEnemy : BasicEnemy
{
    [Header("Boss Stats Mult")]
    public float healthMult;
    public float speedMult;

    private void Update()
    {
        EnemyPathFinding();
        //TryAttack();
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
