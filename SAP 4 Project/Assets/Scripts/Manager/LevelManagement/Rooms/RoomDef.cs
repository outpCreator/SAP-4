using UnityEngine;

[CreateAssetMenu(fileName = "Level/Room")]
public class RoomDef : ScriptableObject
{
    public GameObject roomPrefab;

    public bool isBossRoom = false;
    public bool isStartRoom = false;
}
