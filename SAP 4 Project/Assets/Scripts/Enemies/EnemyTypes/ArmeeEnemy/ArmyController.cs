using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ArmyController : MonoBehaviour
{
    public ArmyEnemy config;
    public GameObject unitPrefab;
    public Transform spawnPoint;

    List<ArmyUnit> units = new List<ArmyUnit>();

    private void Start()
    {
        AdjustUnitCount(config.maxSplits);
    }

    public void AdjustUnitCount(int targetCount)
    {
        int current = units.Count;

        if (targetCount > current)
        {
            int toCreate = targetCount - current;
            for (int i = 0; i < toCreate; i++)
            {
                Vector3 spawnPos = transform.localPosition + transform.right * (i - targetCount / 2f);
                GameObject unitGO = Instantiate(unitPrefab, spawnPos, Quaternion.identity, spawnPoint);
                ArmyUnit unit = unitGO.GetComponent<ArmyUnit>();
                unit.Initialize(this);
                units.Add(unit);

            }
        }
        else if (targetCount < current)
        {
            int toRemove = current - targetCount;
            for (int i = 0;i < toRemove; i++)
            {
                ArmyUnit unit = units[^1];
                units.RemoveAt(units.Count - 1);
                Destroy(unit.gameObject);
            }
        }

        RepositionUnits();
        AdjustUnitScale();
    }

    void RepositionUnits()
    {
        float spacing = 1.5f;
        int count = units.Count;
        float offset = -(count - 1) * spacing / 2f;

        for (int i = 0; i < count; i++)
        {
            Vector3 localPos = new Vector3(offset + i * spacing, 0, 0);
            units[i].transform.localPosition = localPos;
        }
    }

    void AdjustUnitScale()
    {
        int count = units.Count;

        float baseScale = Mathf.Lerp(0.4f, 1f, 1f / count);

        foreach (var unit in units)
        {
            unit.SetScale(Vector3.one * baseScale, 0.3f);
        }
    }

    public void OnUnitDestroyed(ArmyUnit unit)
    {
        units.Remove(unit);
        RepositionUnits();
    }

    public List<ArmyUnit> GetUnits() => units;
}
