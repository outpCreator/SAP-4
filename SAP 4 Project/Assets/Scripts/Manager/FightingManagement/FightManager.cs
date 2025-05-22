using UnityEngine;
using System.Collections.Generic;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;

    // Player Components
    PlayerMovement playerMovement;
    CameraMovement cameraMovement;

    [Header("Enemies")]
    EnemyCombat[] allEnemies;
    public List<EnemyCombat> activeEnemies = new List<EnemyCombat>();

    public enum FightStates
    {
        NoActiveFight,
        InAFight,
    }

    public FightStates state = FightStates.NoActiveFight;

    private void Awake()
    {
        Instance = this;
    }

    public void InitFightManager()
    {
        playerMovement = PlayerManager.Instance.playerMovementScript;
        cameraMovement = PlayerManager.Instance.cameraMovement;
    }

    public void GetEnemies()
    {
        allEnemies = null;
        allEnemies = FindObjectsByType<EnemyCombat>(FindObjectsSortMode.None);

        foreach (EnemyCombat enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.SetUpEnemy();
            }
        }
    }

    private void Update()
    {
        if (playerMovement == null) return;

        UpdateFightState();

        switch (state)
        {
            case FightStates.NoActiveFight:

                cameraMovement.SwitchCameraMode(false);

                break;
            case FightStates.InAFight:

                cameraMovement.SwitchCameraMode(true);

                break;
        }
    }

    void UpdateFightState()
    {
        List<EnemyCombat> cleanedEnemies = new List<EnemyCombat>();

        foreach (var enemy in allEnemies)
        {
            if (enemy == null || enemy.State == EnemyCombat.EnemyState.Died)
                continue;

            if (enemy.State == EnemyCombat.EnemyState.InRange)
            {
                enemy.outOfRangeTimer = 0;

                if (!activeEnemies.Contains(enemy))
                {
                    activeEnemies.Add(enemy);
                }
                cleanedEnemies.Add(enemy);
            }
            else if (activeEnemies.Contains(enemy))
            {
                enemy.outOfRangeTimer += Time.deltaTime;

                if (enemy.outOfRangeTimer < EnemyCombat.maxOutOfRangeTime)
                {
                    cleanedEnemies.Add(enemy);
                }
            }
        }

        activeEnemies = cleanedEnemies;

        if (activeEnemies.Count > 0)
        {
            state = FightStates.InAFight;
        }
        else
        {
            state = FightStates.NoActiveFight;
        }
    }
}
