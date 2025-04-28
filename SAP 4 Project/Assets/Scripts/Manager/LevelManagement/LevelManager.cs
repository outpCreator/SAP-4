using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Rooms")]
    public int completedRooms = 0;
    public int fixedRooms = 0;

    [Header("Grid")]
    public int gridSize = 5;
    public float roomSpacing = 20f;

    private Dictionary<Vector2Int, Vector3> roomPositions;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();
        fixedRooms = Mathf.Clamp(fixedRooms, 0, 2);
    }

    void GenerateGrid()
    {
        roomPositions = new Dictionary<Vector2Int, Vector3>();

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
            return Pos;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public List<Vector2Int> GetEdgePositions()
    {
        List<Vector2Int> edgePositions = new List<Vector2Int>();
        int half = gridSize / 2;

        foreach (var kvp in roomPositions)
        {
            Vector2Int coord = kvp.Key;
            if (Mathf.Abs(coord.x) == half || Mathf.Abs(coord.y) == half)
            {
                edgePositions.Add(coord);
            }
        }

        return edgePositions;
    }

    public List<Vector2Int> GetCenterPositions()
    {
        List<Vector2Int> centerPositions = new List<Vector2Int>();

        foreach (var kvp in roomPositions)
        {
            Vector2Int coord = kvp.Key;
            if (Mathf.Abs(coord.x) <= 1 && Mathf.Abs(coord.y) <= 1)
            {
                centerPositions.Add(coord);
            }
        }

        return centerPositions;
    }
    
    public Dictionary<Vector2Int, Vector3> GetAllRoomPositions()
    {
        return roomPositions;
    }

    Dictionary<Vector2Int, GameObject> spawnedRooms = new Dictionary<Vector2Int, GameObject>();
    public void RegisterRoomInstance(Vector2Int gridCoord, GameObject roomInstance)
    {
        if (!spawnedRooms.ContainsKey(gridCoord))
        {
            spawnedRooms.Add(gridCoord, roomInstance);
        }
    }

    public bool HasRoomAt(Vector2Int coord)
    {
        return spawnedRooms.ContainsKey(coord);
    }

    public Quaternion GetRotation(Vector2Int coord)
    {
        int half = gridSize / 2;

        if (coord.y == half)
        {
            return Quaternion.Euler(0f, 180f, 0f);
        }
        else if (coord.y == -half)
        {
            return Quaternion.Euler(0f, 0f, 0f);
        }
        else if (coord.x == half)
        {
            return Quaternion.Euler(0f, -90f, 0f);
        }
        else if (coord.x == -half)
        {
            return Quaternion.Euler(0f, 90f, 0f);
        }

        return Quaternion.identity;
    }

    private void Update()
    {
        //foreach(GameObject room in spawnedRooms)
        //{
        //    if(room.position == Vector2Int.zero)
        //    {

        //    }
        //}
    }
}
