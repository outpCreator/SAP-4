using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner Instance;

    [Header("Rooms")]
    public List<RoomDef> rooms = new List<RoomDef>();
    public GameObject[] roomPrefabs;
    public GameObject startRoomPrefab;
    public GameObject bossRoomPrefab;

    bool transitioning = false;

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

        LevelManager.Instance.SetCurrentRoomCoord(startCoord);
        LevelManager.Instance.AddFixedRoom(startCoord);

        Transform roomAnchor = startRoom.transform.Find("RoomAnchor");
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.InitPlayer();

            PlayerManager.Instance.SetSpawnPoint(roomAnchor);
        }

        usedCoords.Add(startCoord);

        LevelManager.Instance.SetStartRoomCoord(startCoord);

        // Bossraum
        Vector2Int bossCoord = LevelManager.Instance.GetRandomCenterPositionExcluding(usedCoords);
        Debug.Log($"BossCoord chosen: {bossCoord}");
        Vector3 bossPos = LevelManager.Instance.GetWorldPosition(bossCoord);
        GameObject bossRoom = Instantiate(bossRoomPrefab, bossPos, Quaternion.identity, this.transform);
        bossRoom.name = $"Boss Room ({bossCoord.x}, {bossCoord.y})";
        LevelManager.Instance.RegisterRoomInstance(bossCoord, bossRoom);

        LevelManager.Instance.AddFixedRoom(bossCoord);
    }

    public void SpawnNextRoom(DoorTrigger.Direction doorDirection)
    {
        if (transitioning)
            return;

        transitioning = true;

        Vector2Int currentRoomCoord = LevelManager.Instance.CurrentRoomCoord;
        Vector2Int newRoomCoord = LevelManager.Instance.GetNewRoomPosition(currentRoomCoord, doorDirection);
        Debug.Log($"[LevelSpawner] Moving from {currentRoomCoord} to {newRoomCoord} through {doorDirection} door.");

        if (!LevelManager.Instance.IsValidRoomCoord(newRoomCoord))
        {
            print("Raum liegt nicht auf dem Grid!");
            StartCoroutine(LevelManager.Instance.ReturnToStartRoom());
            return;
        }

        if (LevelManager.Instance.HasRoomAt(newRoomCoord))
        {
            bool isFixedRoom = LevelManager.Instance.IsRoomFixed(newRoomCoord);
            if (isFixedRoom)
            {
                LevelManager.Instance.SetCurrentRoomCoord(newRoomCoord);

                GameObject fixedRoomInstance = LevelManager.Instance.GetRoomInstance(newRoomCoord);
                if (fixedRoomInstance != null)
                {
                    Transform NewRoomAnchor = fixedRoomInstance.transform.Find("RoomAnchor").gameObject.transform;
                    StartCoroutine(PlayerManager.Instance.HandleRoomTransition(NewRoomAnchor.position, NewRoomAnchor.rotation));
                }

                GameObject oldRoomInstance = LevelManager.Instance.GetRoomInstance(currentRoomCoord);
                if (oldRoomInstance != null && !LevelManager.Instance.IsRoomFixed(currentRoomCoord))
                {
                    Destroy(oldRoomInstance);
                    LevelManager.Instance.RemoveRoomInstance(currentRoomCoord, oldRoomInstance);
                }

                StartCoroutine(TransitionDelay());
                return;
            }
            else
            {
                Debug.LogWarning($"[LevelSpawner] Warning: Overwriting non-fixed room at {newRoomCoord}!");
            }
        }
        
        Vector3 roomPos = LevelManager.Instance.GetWorldPosition(newRoomCoord);
        Quaternion roomRot = LevelManager.Instance.GetRotation(newRoomCoord);

        int randomRoom = Random.Range(0, roomPrefabs.Length);

        GameObject newRoom = Instantiate(roomPrefabs[randomRoom], roomPos, roomRot, this.transform);
        newRoom.name = $"Room ({newRoomCoord.x}, {newRoomCoord.y})";

        Transform roomAnchor = newRoom.transform.Find("RoomAnchor").gameObject.transform;

        StartCoroutine(PlayerManager.Instance.HandleRoomTransition(roomAnchor.position, roomAnchor.rotation));

        LevelManager.Instance.RegisterRoomInstance(newRoomCoord, newRoom);

        GameObject roomToDelete = LevelManager.Instance.GetRoomInstance(currentRoomCoord);
        if (roomToDelete != null && !LevelManager.Instance.IsRoomFixed(currentRoomCoord))
        {
            Destroy(roomToDelete);
            LevelManager.Instance.RemoveRoomInstance(currentRoomCoord, roomToDelete);
        }

        LevelManager.Instance.SetCurrentRoomCoord(newRoomCoord);

        StartCoroutine(TransitionDelay());
    }

    IEnumerator TransitionDelay()
    {
        yield return new WaitForSeconds(0.1f);
        transitioning = false;
    }
}
