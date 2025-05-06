using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCombat : MonoBehaviour
{
    [Header("EnemyComponents")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    public EnemyStats stats;

    public Transform homePoint;

    [Header("Fight Settings")]
    public float detectionRange = 10f;
    public float attackRange = 3f;
    public bool CanAct { get; private set; } = false;
    public float baseCoolDown = 9f;
    float cooldownTimer = 0f;

    // Enemy Orbit Movement
    float orbitDirectionChange = 1f;
    float directionChangeTimer = 0f;
    float currentOrbitSpeed = 2f;
    float orbitSpeedMin = 1.5f;
    float orbitSpeedMax = 2.5f;
    float directionChangeInterval = 2f;

    bool isAttacking = false;

    float health = 0;

    public enum EnemyState
    {
        Idle,
        Follow,
        InRange,
        Died,
    }

    public void SetUpEnemy()
    {
        Debug.Log($"Enemy {this.name} was set up!");

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null) player = PlayerManager.Instance.playerTransform;
        if (stats == null) stats = GetComponent<EnemyStats>();
    }

    public EnemyState State = EnemyState.Idle;

    private void Update()
    {
        if (player == null) return;

        health = stats.health;

        Vector3 playerPos = player.position;
        Vector3 enemyPos = transform.position;

        if(health <= 0)
        {
            State = EnemyState.Died;
        }

        switch (State)
        {
            case EnemyState.Idle:

                if (Vector3.Distance(enemyPos, playerPos) < detectionRange)
                {
                    State = EnemyState.Follow;
                }

                agent.SetDestination(homePoint.position);

                // Idle Movement around Homepoint

                break;
            case EnemyState.Follow:

                agent.SetDestination(playerPos);

                if (Vector3.Distance(enemyPos, playerPos) <= attackRange)
                {
                    State = EnemyState.InRange;
                }

                if (Vector3.Distance(enemyPos, playerPos) > detectionRange)
                {
                    State = EnemyState.Idle;
                }

                break;
            case EnemyState.InRange:

                if (Vector3.Distance(enemyPos, playerPos) > attackRange)
                {
                    cooldownTimer = baseCoolDown;
                    CanAct = false;
                    isAttacking = false;

                    State = EnemyState.Follow;
                }
                else
                {
                    if (!isAttacking)
                    {
                        directionChangeTimer -= Time.deltaTime;
                        if (directionChangeTimer <= 0)
                        {
                            orbitDirectionChange *= Random.value > 0.5f ? 1 : -1;
                            currentOrbitSpeed = Random.Range(orbitSpeedMin, orbitSpeedMax);
                            directionChangeTimer = directionChangeInterval + Random.Range(-1, 1);
                        }

                        Vector3 targetDirection = (enemyPos - playerPos).normalized;
                        Vector3 orbitDirection = Quaternion.Euler(0, 90 * orbitDirectionChange, 0) * targetDirection;
                        Vector3 orbitTarget = enemyPos + orbitDirection * currentOrbitSpeed;

                        agent.SetDestination(orbitTarget);
                    }
                    else
                    {
                        agent.SetDestination(playerPos);
                    }

                    // Cooldown and round Management

                    if (!CanAct)
                    {
                        cooldownTimer -= Time.deltaTime;
                        if (cooldownTimer < 0)
                        {
                            CanAct = true;
                        }
                    }
                    else if (CanAct)
                    {
                        CanAct = false;
                        cooldownTimer = baseCoolDown;

                        isAttacking = true;

                        Debug.Log($"{name} greift an!");

                        StartCoroutine(FinishAttack());
                    }

                }

                break;
            case EnemyState.Died:

                OnEnemyDied();

                break;
        }
    }

    IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(1);

        isAttacking = false;
    }

    void OnEnemyDied()
    {
        // Enemy drops loot

        Destroy(gameObject);
    }
}
