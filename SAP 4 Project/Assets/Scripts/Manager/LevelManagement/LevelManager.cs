using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int completedRooms = 0;
    public int fixedRooms = 0;

    [Header("Grid")]
    public List<RoomAnker> spawnedRooms = new List<RoomAnker>();
    public float horizontalSpacing = 20f;
    public float verticalSpacing = 20f;

    private void Awake()
    {
        Instance = this;
        fixedRooms = Mathf.Clamp(fixedRooms, 0, 2);
    }

    private void Update()
    {
        foreach(RoomAnker room in spawnedRooms)
        {
            if(room.position == Vector2Int.zero)
            {

            }
        }
    }
}
