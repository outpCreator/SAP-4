using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Rooms")]
    public int completedRooms = 0;
    public int FixedRooms = 0;

    Vector2Int currentRoomCoord;
    public Vector2Int CurrentRoomCoord => currentRoomCoord;

    [Header("Grid")]
    public int gridSize = 5;
    public float roomSpacing = 20f;

    Dictionary<Vector2Int, Vector3> roomPositions = new Dictionary<Vector2Int, Vector3>();
    Dictionary<Vector2Int, GameObject> spawnedRooms = new Dictionary<Vector2Int, GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FixedRooms = Mathf.Clamp(FixedRooms, 0, 2);
    }

    public void InitLevel()
    {
        GenerateGrid();
        spawnedRooms.Clear();
    }

    public void GenerateGrid()
    {
        int half = gridSize / 2;

        for (int x = 0 - half; x <= half; x++)
        {
            for (int y = 0 - half; y <= half; y++)
            {
                Vector2Int gridCoord = new Vector2Int(x, y);
                Vector3 worldPos = new Vector3(x * roomSpacing, 0f, y * roomSpacing);
                roomPositions.Add(gridCoord, worldPos);
            }
        }
    }

    public Vector3 GetWorldPosition(Vector2Int gridCoord)
    {
        if (roomPositions.TryGetValue(gridCoord, out Vector3 Pos))
        {
            Debug.Log($"WorldPosition für {gridCoord}: {Pos}");
            return Pos;
        }
        else
        {
            Debug.LogWarning($"WorldPosition für {gridCoord} NICHT gefunden!");
            return Vector3.zero;
        }
    }

    public Dictionary<Vector2Int, Vector3> GetAllRoomPositions()
    {
        return roomPositions;
    }

    public void SetCurrentRoomCoord(Vector2Int newCoord)
    {
        currentRoomCoord = newCoord;
    }

    public bool IsValidRoomCoord(Vector2Int coord)
    {
        int half = gridSize / 2;

        if (coord.x < -half || coord.x > half || coord.y < -half || coord.y > half)
        {
            return false;
        }

        return true;
    }

    public bool HasRoomAt(Vector2Int coord)
    {
        return spawnedRooms.ContainsKey(coord);
    }

    public List<Vector2Int> GetEdgePositions()
    {
        List<Vector2Int> edgePositions = new List<Vector2Int>();
        int half = gridSize / 2;

        for (int x = -half; x <= half; x++)
        {
            for (int y = -half; y <= half; y++)
            {
                if (Mathf.Abs(x) == half || Mathf.Abs(y) == half)
                {
                    edgePositions.Add(new Vector2Int(x, y));
                }
            }
        }

        return edgePositions;
    }

    public Vector2Int GetRandomEdgePosition()
    {
        var edges = GetEdgePositions();
        return edges[Random.Range(0, edges.Count)];
    }

    public Vector2Int GetRandomEdgePositionExcluding(List<Vector2Int> exclude)
    {
        var edges = GetEdgePositions();
        edges.RemoveAll(pos => exclude.Contains(pos));
        return edges[Random.Range(0, edges.Count)];
    }

    public List<Vector2Int> GetCenterPositions()
    {
        List<Vector2Int> centerPositions = new List<Vector2Int>();
        int half = gridSize / 2;

        for (int x = -half; x <= half; x++)
        {
            for (int y = -half; y <= half; y++)
            {
                if (Mathf.Abs(x) <= 1 && Mathf.Abs(y) <= 1)
                {
                    centerPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        return centerPositions;
    }

    public Vector2Int GetRandomCenterPosition()
    {
        var centers = GetCenterPositions();
        return centers[Random.Range(0, centers.Count)];
    }

    public Vector2Int GetRandomCenterPositionExcluding(List<Vector2Int> exclude)
    {
        var centers = GetCenterPositions();
        centers.RemoveAll(pos => exclude.Contains(pos));
        return centers[Random.Range(0, centers.Count)];
    }

    public Vector2Int GetDirectionOffset(DoorTrigger.Direction dir)
    {
        switch (dir)
        {
            case DoorTrigger.Direction.North:
                return new Vector2Int(0, 1);
            case DoorTrigger.Direction.South:
                return new Vector2Int(0, -1);
            case DoorTrigger.Direction.East:
                return new Vector2Int(1, 0);
            case DoorTrigger.Direction.West:
                return new Vector2Int(-1, 0);
            default:
                return Vector2Int.zero;
        }
    }

    public Vector2Int GetNewRoomPosition(Vector2Int currentCoord, DoorTrigger.Direction direction)
    {
        Vector2Int offset = GetDirectionOffset(direction);
        return currentCoord + offset;
    }

    public void RegisterRoomInstance(Vector2Int gridCoord, GameObject roomInstance)
    {
        if (!spawnedRooms.ContainsKey(gridCoord))
        {
            spawnedRooms.Add(gridCoord, roomInstance); // <-- Vielleicht noch transform.position!!
        }
    }

    public void RemoveRoomInstance(Vector2Int gridCoord, GameObject roomInstance)
    {
        if (spawnedRooms.ContainsKey(gridCoord))
        {
            spawnedRooms.Remove(gridCoord);
        }
    }

    public GameObject GetRoomInstance(Vector2Int coord)
    {
        if (spawnedRooms.TryGetValue(coord, out GameObject roomInstance))
        {
            return roomInstance;
        }

        return null;
    }

    public Quaternion GetRotation(Vector2Int coord)
    {
        int half = gridSize / 2;

        if (coord.y == half)
            return Quaternion.Euler(0f, 180f, 0f);
        else if (coord.y == -half)
            return Quaternion.Euler(0f, 0f, 0f);
        else if (coord.x == half)
            return Quaternion.Euler(0f, -90f, 0f);
        else if (coord.x == -half)
            return Quaternion.Euler(0f, 90f, 0f);

        return Quaternion.identity;
    }

    HashSet<Vector2Int> fixedRooms = new HashSet<Vector2Int>();
    public bool IsRoomFixed(Vector2Int coord)
    {
        return fixedRooms.Contains(coord);
    }

    public void AddFixedRoom(Vector2Int coord)
    {
        if (!fixedRooms.Contains(coord))
        {
            fixedRooms.Add(coord);
        }
    }
}
