using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SmallEnemyBehaviour : MonoBehaviour
{
    EnemyCombat combat;

    [Header("Idle Settings")]
    [SerializeField] float idleRange = 3f;
    [SerializeField] float idleSpeed = 4f;

    Vector3 idleTarget;
    bool idle = false;

    [Header("Follow Settings")]
    [SerializeField] float chaseTime = 3f;
    float waitTimer = 0f;
    bool chase = false;

    public void Initialize(EnemyCombat enemyCombat)
    {
        combat = enemyCombat;
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
        if (!chase)
        {
            waitTimer += Time.deltaTime;
            while (waitTimer < chaseTime)
            {
                // Agent flieht vor dem Spieler
            }

            if (waitTimer >= chaseTime)
            {
                chase = true;
            }

            //if (Check vor Damage Taken)
            //{
                // Agent charges at player && gets first Turn => FightManager
            //}
        }
        else
        {
            combat.agent.SetDestination(combat.playerPosition);
        }
    }

    public void InRangeBehaviour()
    {
        // Movement = Circle around Player

        // If (Enemy has Turn) => Choose random Attack
    }

    public void DiedBehaviour()
    {

    }
}
