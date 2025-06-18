using System.Collections.Generic;
using UnityEngine;

public class LevelRoom : MonoBehaviour
{
    public GameObject chest;

    public bool enemiesCleared;

    private void Start()
    {
        chest.SetActive(false);
    }

    private void Update()
    {
        if (enemiesCleared)
        {
            chest.SetActive(true);
            PlayerManager.Instance.IncreaseCompletedRoomsCount();
        }
    }
}
