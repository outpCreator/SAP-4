using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SmallEnemyBehaviour : MonoBehaviour
{
    EnemyCombat combat;
    EnemyStats stats;

    [Header("Idle Settings")]
    [SerializeField] float idleRange = 3f;
    [SerializeField] float idleSpeed = 4f;

    Vector3 idleTarget;
    bool idle = false;

    [Header("Follow Settings")]
    [SerializeField] float chaseTime = 3f;
    float waitTimer = 0f;
    bool chase = false;

    [Header("InRange Settings")]
    [SerializeField] float directionChangeTimer = 1f;
    [SerializeField] float orbitDirectionChange = 1f;
    [SerializeField] float currentOrbitSpeed = 2f;
    [SerializeField] float directionChangeInterval = 2f;

    [SerializeField] float orbitSpeedMin = 1.5f;
    [SerializeField] float orbitSpeedMax = 2.5f;

    public bool CanAct { get; private set; } = false;

    bool isAttacking = false;
    bool isOrbiting = false;

    float coolDownTimer = 0f;

    public void Initialize(EnemyCombat enemyCombat, EnemyStats enemyStats)
    {
        combat = enemyCombat;
        stats = enemyStats;
    }

    public void IdleBehaviour()
    {
        chase = false;

        if (!idle)
        {
            idle = true;

            float randomX = Random.Range(-idleRange, idleRange);
            float randomZ = Random.Range(-idleRange, idleRange);

            idleTarget = combat.homePoint.position + new Vector3(randomX, 0, randomZ);
            combat.agent.SetDestination(idleTarget);

            StartCoroutine(IdleRoutine());
        }
    }

    IEnumerator IdleRoutine()
    {
        yield return new WaitForSeconds(4);

        idle = false;
    }

    public void FollowBehaviour()
    {
        combat.agent.SetDestination(combat.playerPosition);
    }

    public void InRangeBehaviour()
    {
        if (!isAttacking)
        {
            directionChangeTimer -= Time.deltaTime;
            if (directionChangeTimer <= 0)
            {
                orbitDirectionChange *= Random.value > 0.5f ? 1 : -1;
                currentOrbitSpeed = Random.Range(orbitSpeedMin, orbitSpeedMax);
                directionChangeTimer = directionChangeInterval + Random.Range(-1, 1);
                isOrbiting = Random.value > 0.3f;
            }

            if (isOrbiting)
            {
                Vector3 targetDirection = (combat.enemyPosition - combat.playerPosition).normalized;
                Vector3 orbitDirection = Quaternion.Euler(0, 90 * orbitDirectionChange, 0) * targetDirection;
                Vector3 orbitTarget = combat.enemyPosition + orbitDirection * currentOrbitSpeed;

                combat.agent.SetDestination(orbitTarget);
            }
            else
            {
                combat.agent.SetDestination(combat.enemyPosition);
            }
        }
        else
        {
            Vector3 rangePos = (combat.playerPosition - combat.enemyPosition).normalized;
            Vector3 attackPos = combat.playerPosition - rangePos * (stats.attackRange * 0.9f);

            combat.agent.SetDestination(attackPos);
        }

        if (!CanAct)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer < 0)
            {
                CanAct = true;
            }
        }
        else if (CanAct)
        {
            CanAct = false;
            coolDownTimer = stats.attackCooldown;

            isAttacking = true;

            float prepTime = Random.Range(0.3f, 0.8f);
            StartCoroutine(FinishAttack(prepTime));
        }
    }

    IEnumerator FinishAttack(float delay)
    {
        yield return new WaitForSeconds(delay);

        print("Angriff");

        yield return new WaitForSeconds(1);

        isAttacking = false;
    }

    public void DiedBehaviour()
    {

    }
}
