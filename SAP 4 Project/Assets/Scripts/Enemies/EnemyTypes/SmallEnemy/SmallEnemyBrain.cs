using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI/SmallEnemyBrain")]
public class SmallEnemyBrain : EnemyBehaviourDefinition
{

    public override void Action(SmallEnemyBehaviour behaviour,  EnemyCombat.EnemyState state)
    {
        if (state == EnemyCombat.EnemyState.Idle)
        {
            behaviour.IdleBehaviour();
        }
        else if (state == EnemyCombat.EnemyState.Follow)
        {
            behaviour.FollowBehaviour();
        }
        else if (state == EnemyCombat.EnemyState.InRange)
        {
            behaviour.InRangeBehaviour();
        }
        else if (state == EnemyCombat.EnemyState.Died)
        {
            behaviour.DiedBehaviour();
        }
    }
}
