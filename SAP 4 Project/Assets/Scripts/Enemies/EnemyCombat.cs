using UnityEngine;
using UnityEngine.AI;

public class EnemyCombat : MonoBehaviour
{
    [Header("Enemy")]
    public EnemyBehaviour behaviour;
    public EnemyStats stats;
    public NavMeshAgent agent;
    public Transform homePoint;
    public float health;

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

        behaviour.Initialize(this, stats);

        health = stats.health;
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 playerPos = player.position;
        playerPosition = playerPos;
        Vector3 enemyPos = transform.position;

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

                behaviour.Action(State);

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

                behaviour.Action(State);

                break;

            case EnemyState.InRange:

                if (Vector3.Distance(enemyPos, playerPos) > stats.attackRange)
                {
                    State = EnemyState.Follow;
                }

                behaviour.Action(State);

                break;

            case EnemyState.Died:

                behaviour.Action(State);

                break;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }
}
