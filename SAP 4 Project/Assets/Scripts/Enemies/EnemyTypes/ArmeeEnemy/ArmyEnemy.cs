using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI/SmarmyEnemy")]
public class ArmyEnemy : EnemyBehaviour
{
    [Header("Army Settings")]
    public int maxSplits = 5;
    public int minSplits = 1;

    public override void Initialize(EnemyCombat enemyCombat, EnemyStats stats)
    {
        
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
