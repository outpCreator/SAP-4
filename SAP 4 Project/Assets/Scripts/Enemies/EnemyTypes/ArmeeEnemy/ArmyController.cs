using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class ArmyController : MonoBehaviour
{
    EnemyCombat combat;
    public GameObject unitPrefab;
    public Transform spawnPoint;

    [Header("Unit Settings")]
    [SerializeField] float maxScale = 1f;
    [SerializeField] float minScale = 0.4f;

    List<ArmyUnit> units = new List<ArmyUnit>();

    [Header("Patroulling")]
    [SerializeField] List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] float patrolWaitTime = 2f;

    int patrolIndex = 0;
    float waitTimer = 0;
    bool isWaiting = false;



    public void Initialize(EnemyCombat enemyCombat)
    {
        combat = enemyCombat;
    }

    public void IdleBehaviour()
    {
        AdjustUnitCount(5);

        if (patrolPoints == null || patrolPoints.Count == 0) return;

        Transform currentTarget = patrolPoints[patrolIndex];

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                isWaiting = false;
                waitTimer = 0;

                patrolIndex = (patrolIndex + 1) % patrolPoints.Count;
            }
            return;
        }

        combat.agent.SetDestination(currentTarget.position);

        float distance = Vector3.Distance(transform.position, currentTarget.position);
        if (distance < 1f)
        {
            isWaiting = true;
        }
    }

    public void FollowBehaviour()
    {
        combat.agent.SetDestination(combat.playerPosition);
    }

    public void InRangeBehaviour()
    {

    }

    public void DiedBehaviour()
    {

    }

    public void AdjustUnitCount(int targetCount)
    {
        while (units.Count > targetCount)
        {
            Destroy(units[^1].gameObject);
            units.RemoveAt(units.Count - 1);
        }

        while (units.Count < targetCount)
        {
            int index = units.Count;

            Vector3 spawnOffset = -spawnPoint.forward * index * 2;
            Vector3 spawnPos = spawnPoint.position + spawnOffset;

            GameObject unitGO = Instantiate(unitPrefab, spawnPos, Quaternion.identity, spawnPoint);
            ArmyUnit unit = unitGO.GetComponent<ArmyUnit>();

            Transform followTarget = (index == 0) ? null : units[index - 1].transform;
            unit.Initialize(this, followTarget);

            units.Add(unit);
        }

        AdjustUnitScale();
    }
    
    void AdjustUnitScale()
    {
        int count = Mathf.Max(units.Count, 1);
        float t = Mathf.Clamp01(1f / count);
        float scale = Mathf.Lerp(minScale, maxScale, t);

        Vector3 targetScale = Vector3.one * scale;

        foreach (var unit in units)
        {
            unit.SetScale(targetScale, 0.3f);
        }
    }
}
