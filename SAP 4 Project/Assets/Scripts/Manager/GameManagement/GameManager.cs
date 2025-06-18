using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameObject spawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.InitLevel();
        }

        if (LevelSpawner.Instance != null)
        {
            LevelSpawner.Instance.InitLevel();
        }

        if (LevelManager.Instance == null || LevelSpawner.Instance == null && PlayerManager.Instance != null)
        {
            Debug.Log("Not in a Level! Initialized from Game Manger");

            spawnPoint = GameObject.FindWithTag("SpawnPoint");

            PlayerManager.Instance.InitPlayer();
            PlayerManager.Instance.SetSpawnPoint(spawnPoint.transform);
        }

        if (FightManager.Instance != null)
        {
            Debug.Log("Fight Manager is not null!");
            FightManager.Instance.InitFightManager();
            FightManager.Instance.GetEnemies();
        }
    }
}
