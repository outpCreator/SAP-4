using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner Instance;

    public Transform roomAnker;

    [Header("Rooms")]
    public GameObject[] rooms;

    float tileSize = 20f;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnNextRoom(Vector3 spawnPosition)
    {
        foreach (var room in rooms)
        {
            int roomNumber = Random.Range(0, rooms.Length);

            GameObject roomToSpawn = rooms[roomNumber];
            GameObject activeRoom = Instantiate(roomToSpawn, spawnPosition, Quaternion.identity);

            roomAnker = activeRoom.GetComponentInChildren<RoomAnker>().transform;
        }
    }
}
