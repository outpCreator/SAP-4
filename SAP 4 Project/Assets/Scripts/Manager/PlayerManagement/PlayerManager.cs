using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject playerPrefab;
    GameObject playerInstance;

    private void Awake()
    {
        Instance = this;
    }

    public void InitPlayer()
    {
        playerInstance = Instantiate(playerPrefab);

        SceneLoader.Instance.onSceneChanged.AddListener(OnSceneLoaded);
    }

    public void SetSpawnPoint(Transform anchor)
    {
        if (anchor != null)
        {
            if (playerInstance != null)
            {
                playerInstance.GetComponent<CursorManager>().OnAfterSpawn(anchor.position, anchor.rotation);
            }
            else
            {
                Debug.LogError("Instance ist Null!");
            }
        }
        else
        {
            Debug.LogError("Anker ist Null");
        }
    }

    public void MovePlayerContainer(Transform anchor, DoorTrigger.Direction direction)
    {
        StartCoroutine(MovePlayer(anchor, direction));
    }

    IEnumerator MovePlayer(Transform anchor, DoorTrigger.Direction direction)
    {
        float time = 0f;
        float duration = 1f;

        Vector3 startPos = playerInstance.transform.position;
        Vector3 targetPos = anchor.position;

        Quaternion startRot = playerInstance.transform.rotation;
        Quaternion targetRot = GetPlayerRotation(direction);

        PlayerMovement playerMovement = playerInstance.GetComponentInChildren<PlayerMovement>();
        if (playerMovement != null )
        {
            playerMovement.FreezeMovement(true);
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            playerInstance.transform.position = Vector3.Lerp(startPos, targetPos, t);
            playerInstance.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        playerInstance.transform.position = targetPos;
        playerInstance.transform.rotation = targetRot;

        if (playerMovement != null )
        {
            playerMovement.FreezeMovement(false);
            playerMovement.AlignMoveDirection(targetRot);
        }
    }

    Quaternion GetPlayerRotation(DoorTrigger.Direction direction)
    {
        switch (direction)
        {
            case DoorTrigger.Direction.North:
                return Quaternion.Euler(0, 0, 0);
            case DoorTrigger.Direction.East:
                return Quaternion.Euler(0, 90, 0);
            case DoorTrigger.Direction.South:
                return Quaternion.Euler(0, 180, 0);
            case DoorTrigger.Direction.West:
                return Quaternion.Euler(0, 270, 0);
            default:
                return Quaternion.identity;
        }
    }

    public void OnSceneLoaded(string entryId)
    {
        GameObject spawnPoint = GameObject.Find(entryId);
        if(spawnPoint != null)
        {
            playerInstance.GetComponentInChildren<PlayerMovement>().OnAfterSpawn(spawnPoint.transform.position, spawnPoint.transform.rotation);
        } 
        else
        {
            Debug.LogError("Spawn point missing! Please add one.");
        }
    }

    private void OnDestroy()
    {
        SceneLoader.Instance.onSceneChanged.RemoveListener(OnSceneLoaded);
    }
}
