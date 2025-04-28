using System.Security.Cryptography;
using UnityEngine;

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
