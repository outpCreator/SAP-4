using System.Collections.Generic;
using UnityEngine;

public class LevelRoom : MonoBehaviour
{
    public GameObject chest;

    public List<GameObject> enemies = new List<GameObject>();
    int enemieCount;

    private void Start()
    {
        chest.SetActive(false);
    }

    private void Update()
    {
        enemieCount = enemies.Count;

        if (enemieCount <= 0)
        {
            chest.SetActive(true);
        }
    }
}
