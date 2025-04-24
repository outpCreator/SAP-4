using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int completedRooms = 0;

    private void Awake()
    {
        Instance = this;
    }
}
