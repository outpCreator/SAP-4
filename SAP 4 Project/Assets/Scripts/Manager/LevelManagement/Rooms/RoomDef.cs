using UnityEngine;

[CreateAssetMenu(fileName = "Level/Room")]
public class RoomDef : ScriptableObject
{
    public GameObject roomPrefab;

    public int spawnRate = 0;
}
