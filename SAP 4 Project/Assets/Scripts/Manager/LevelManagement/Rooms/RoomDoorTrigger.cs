using UnityEngine;

public class RoomDoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelSpawner.Instance.SpawnNextRoom();
        }
    }
}
