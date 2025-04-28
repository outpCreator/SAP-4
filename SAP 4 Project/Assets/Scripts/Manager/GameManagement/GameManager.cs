using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

        //if(PlayerManager.Instance != null)
        //{
        //    PlayerManager.Instance.InitPlayer();
        //}
    }
}
