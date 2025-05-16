using System.Collections;
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

    [Header("Player")]
    public Transform player;
    public Vector3 playerPosition;

    public enum EnemyState { Idle, Follow, InRange, Attack, Died}
    public EnemyState State = EnemyState.Idle;

    public void SetUpEnemy()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null) player = PlayerManager.Instance.playerTransform;
        if (stats == null) stats = GetComponent<EnemyStats>();

        behaviour.Initialize(this, stats);

        health = stats.health;

        Debug.Log($"Enemy {this.name} was set up!");
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

                behaviour.Idle();

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

                behaviour.Follow();

                break;

            case EnemyState.InRange:

                behaviour.InRange();

                break;

            case EnemyState.Attack:

                behaviour.Attack();

                break;

            case EnemyState.Died:

                behaviour.Die();

                break;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }
}
