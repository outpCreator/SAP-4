using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public enum Direction { North, South, East, West }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Direction actualDirection = GetRotatedDirection();
            LevelSpawner.Instance.SpawnNextRoom(actualDirection);
        }
    }

    Direction GetRotatedDirection()
    {
        Vector3 forward = transform.forward;

        if (Vector3.Dot(forward, Vector3.forward) > 0.7f)
            return Direction.North;
        if (Vector3.Dot(forward, Vector3.right) > 0.7f)
            return Direction.East;
        if (Vector3.Dot(forward, Vector3.back) > 0.7f)
            return Direction.South;
        if (Vector3.Dot(forward, Vector3.left) > 0.7f)
            return Direction.West;

        Debug.LogWarning("Tür konnte keine Richtung anhand ihrer eigenen Ausrichtung bestimmen!");
        return Direction.North;
    }
}
