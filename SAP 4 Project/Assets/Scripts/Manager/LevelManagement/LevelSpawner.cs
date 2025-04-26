using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner Instance;

    public Transform roomAnker;

    [Header("Rooms")]
    public List<GameObject> rooms = new List<GameObject>();
    public GameObject[] availableRooms;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnNextRoom(Vector3 spawnPosition)
    {
        // Get instance of current room

        // Check for fixed Room

        // Check for grid space

        // Get spawnDirection

        // Randomly select room and spawn

        // Set current room to previouse room

        // Set anker of current room

        // Move previouse room to DeletePreviouseRoom()
    }

    void DeletePreviouseRoom(GameObject previouseRoom)
    {
        // Check for fixed room

        // Delete room
    }
}
