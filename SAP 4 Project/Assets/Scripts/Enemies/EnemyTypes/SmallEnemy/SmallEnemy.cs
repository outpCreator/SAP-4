using UnityEngine;

public class SmallEnemy : EnemyBehaviour
{
    EnemyCombat combat;
    EnemyStats stats;

    public override void Initialize(EnemyCombat enemyCombat, EnemyStats enemyStats)
    {
        combat = enemyCombat;
        stats = enemyStats;
    }

    public override void Idle()
    {
        
    }

    public override void Follow()
    {
        
    }

    public override void InRange()
    {
        
    }

    public override void Attack()
    {
        
    }

    public override void Die()
    {
        
    }
}
