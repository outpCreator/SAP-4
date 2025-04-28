using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner Instance;

    [Header("Rooms")]
    public GameObject roomPrefab;
    public GameObject startRoomPrefab;
    public GameObject bossRoomPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void InitLevel()
    {
        SpawnSpecialRooms();
    }

    void SpawnSpecialRooms()
    {
        List<Vector2Int> usedCoords = new List<Vector2Int>();

        // Startraum
        Vector2Int startCoord = LevelManager.Instance.GetRandomEdgePosition();
        Debug.Log($"StartCoord chosen: {startCoord}");
        Vector3 startPos = LevelManager.Instance.GetWorldPosition(startCoord);
        Quaternion startRotation = LevelManager.Instance.GetRotation(startCoord);
        GameObject startRoom = Instantiate(startRoomPrefab, startPos, startRotation, this.transform);
        startRoom.name = $"Start Room ({startCoord.x}, {startCoord.y})";
        LevelManager.Instance.RegisterRoomInstance(startCoord, startRoom);

        Transform roomAnchor = startRoom.transform.Find("RoomAnchor");
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.InitPlayer();

            PlayerManager.Instance.SetSpawnPoint(roomAnchor);
        }

        usedCoords.Add(startCoord);

        // Bossraum
        Vector2Int bossCoord = LevelManager.Instance.GetRandomCenterPositionExcluding(usedCoords);
        Debug.Log($"BossCoord chosen: {bossCoord}");
        Vector3 bossPos = LevelManager.Instance.GetWorldPosition(bossCoord);
        GameObject bossRoom = Instantiate(bossRoomPrefab, bossPos, Quaternion.identity, this.transform);
        bossRoom.name = $"Boss Room ({bossCoord.x}, {bossCoord.y})";
        LevelManager.Instance.RegisterRoomInstance(bossCoord, bossRoom);

        //SpawnNormalRooms(startCoord, bossCoord);
    }

    void SpawnNormalRooms(Vector2Int startCoord, Vector2Int bossCoord)
    {
        var allPositions = LevelManager.Instance.GetAllRoomPositions();

        foreach (var kvp in allPositions)
        {
            Vector2Int coord = kvp.Key;

            if (coord == startCoord || coord == bossCoord) continue;

            if (LevelManager.Instance.HasRoomAt(coord)) continue;

            Vector3 pos = kvp.Value;
            GameObject normalRoom = Instantiate(roomPrefab, pos, Quaternion.identity, this.transform);
            normalRoom.name = $"Normal Room ({coord.x}, {coord.y})";
            LevelManager.Instance.RegisterRoomInstance(coord, normalRoom);
        }
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
