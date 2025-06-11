using UnityEngine;
using UnityEngine.AI;
using static EnemyCombat;

public class EnemyCombat : MonoBehaviour
{
    [Header("Enemy")]
    public EnemyBehaviourDefinition behaviour;
    public SmallEnemyBehaviour enemyBehaviour;
    public EnemyStats stats;
    public NavMeshAgent agent;
    public Transform homePoint;
    public float health;

    public Vector3 enemyPosition;

    public float outOfRangeTimer = 0f;
    public static float maxOutOfRangeTime = 1f;

    [Header("Player")]
    public Transform player;
    public Vector3 playerPosition;

    public enum EnemyState { Idle, Follow, InRange, Died}
    public EnemyState State = EnemyState.Idle;

    public void SetUpEnemy()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null) player = PlayerManager.Instance.playerTransform;
        if (stats == null) stats = GetComponent<EnemyStats>();
        enemyBehaviour = GetComponent<SmallEnemyBehaviour>();

        health = stats.health;
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 playerPos = player.position;
        playerPosition = playerPos;
        Vector3 enemyPos = transform.position;
        enemyPosition = enemyPos;

        if (health <= 0)
        {
            State = EnemyState.Died;
        }

        switch (State)
        {
            case EnemyState.Idle:

                if (Vector3.Distance(enemyPos, playerPos) < stats.detectionRange)
                {
                    State = EnemyState.Follow;
                }

                behaviour.Action(enemyBehaviour, State );

                break;

            case EnemyState.Follow:

                if (Vector3.Distance(enemyPos, playerPos) <= stats.attackRange)
                {
                    State = EnemyState.InRange;
                }

                if (Vector3.Distance(enemyPos, playerPos) > stats.detectionRange)
                {
                    State = EnemyState.Idle;
                }

                behaviour.Action(enemyBehaviour, State);

                break;

            case EnemyState.InRange:

                if (Vector3.Distance(enemyPos, playerPos) > stats.attackRange)
                {
                    State = EnemyState.Follow;
                }
                else
                {
                    behaviour.Action(enemyBehaviour, State);
                }
                

                break;

            case EnemyState.Died:

                behaviour.Action(enemyBehaviour, State);

                break;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }
}
