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

        LevelManager.Instance.SetCurrentRoomCoord(startCoord);
        LevelManager.Instance.AddFixedRoom(startCoord);
        LevelManager.Instance.RegisterRoomInstance(startCoord, startRoom);

        LevelManager.Instance.SetStartRoomCoord(startCoord);

        usedCoords.Add(startCoord);
        Transform roomAnchor = startRoom.transform.Find("RoomAnchor");

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.InitPlayer();

            PlayerManager.Instance.SetSpawnPoint(roomAnchor);
        }

        // Bossraum
        Vector2Int bossCoord = LevelManager.Instance.GetRandomCenterPositionExcluding(usedCoords);
        Debug.Log($"BossCoord chosen: {bossCoord}");

        Vector3 bossPos = LevelManager.Instance.GetWorldPosition(bossCoord);

        GameObject bossRoom = Instantiate(bossRoomPrefab, bossPos, Quaternion.identity, this.transform);
        bossRoom.name = $"Boss Room ({bossCoord.x}, {bossCoord.y})";

        LevelManager.Instance.AddFixedRoom(bossCoord);
        LevelManager.Instance.RegisterRoomInstance(bossCoord, bossRoom);

        bossRoom.SetActive(false);
    }

    public void SpawnNextRoom(DoorTrigger.Direction doorDirection)
    {
        if (transitioning)
            return;

        transitioning = true;

        Vector2Int currentRoomCoord = LevelManager.Instance.CurrentRoomCoord;
        Vector2Int newRoomCoord = LevelManager.Instance.GetNewRoomPosition(currentRoomCoord, doorDirection);
        Debug.Log($"[LevelSpawner] Moving from {currentRoomCoord} to {newRoomCoord} through {doorDirection} door.");

        GameObject previouseRoomInstance = LevelManager.Instance.GetRoomInstance(currentRoomCoord);

        // Check for valid room
        if (!LevelManager.Instance.IsValidRoomCoord(newRoomCoord))
        {
            print("Raum liegt nicht auf dem Grid!");

            GameObject startRoom = LevelManager.Instance.GetRoomInstance(LevelManager.Instance.StartRoomCoord);
            startRoom.SetActive(true);

            StartCoroutine(LevelManager.Instance.ReturnToStartRoom());
            StartCoroutine(TransitionDelay());
            return;
        }

        // Check for existing or fixed room
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

                    fixedRoomInstance.SetActive(true);

                    StartCoroutine(LevelManager.Instance.HandleRoomTransition(NewRoomAnchor.position, NewRoomAnchor.rotation));
                }

                if (previouseRoomInstance != null && !LevelManager.Instance.IsRoomFixed(currentRoomCoord))
                {
                    Destroy(previouseRoomInstance);
                    LevelManager.Instance.RemoveRoomInstance(currentRoomCoord, previouseRoomInstance);
                }
                else if (LevelManager.Instance.IsRoomFixed(currentRoomCoord))
                {
                    previouseRoomInstance.SetActive(false);
                }

                StartCoroutine(TransitionDelay());
                return;
            }
            else
            {
                Debug.LogWarning($"[LevelSpawner] Warning: Overwriting non-fixed room at {newRoomCoord}!");
            }
        }
        
        // New room spawning
        Vector3 roomPos = LevelManager.Instance.GetWorldPosition(newRoomCoord);
        Quaternion roomRot = LevelManager.Instance.GetRotation(newRoomCoord);

        int randomRoom = Random.Range(0, roomPrefabs.Length);

        GameObject newRoom = Instantiate(roomPrefabs[randomRoom], roomPos, roomRot, this.transform);
        newRoom.name = $"Room ({newRoomCoord.x}, {newRoomCoord.y})";

        Transform roomAnchor = newRoom.transform.Find("RoomAnchor").gameObject.transform;

        StartCoroutine(LevelManager.Instance.HandleRoomTransition(roomAnchor.position, roomAnchor.rotation));

        LevelManager.Instance.RegisterRoomInstance(newRoomCoord, newRoom);

        if (previouseRoomInstance != null && !LevelManager.Instance.IsRoomFixed(currentRoomCoord))
        {
            Destroy(previouseRoomInstance);
            LevelManager.Instance.RemoveRoomInstance(currentRoomCoord, previouseRoomInstance);
        }
        else if (LevelManager.Instance.IsRoomFixed(currentRoomCoord))
        {
            previouseRoomInstance.SetActive(false);
        }

        LevelManager.Instance.SetCurrentRoomCoord(newRoomCoord);

        FightManager.Instance.GetEnemies();

        StartCoroutine(TransitionDelay());
    }

    IEnumerator TransitionDelay()
    {
        yield return new WaitForSeconds(0.1f);
        transitioning = false;
    }
}
