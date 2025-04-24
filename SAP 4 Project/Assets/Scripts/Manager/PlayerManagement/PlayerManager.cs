using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject playerPrefab;
    GameObject playerInstance;

    private void Awake()
    {
        Instance = this;
        playerInstance = Instantiate(playerPrefab);

        SceneLoader.Instance.onSceneChanged.AddListener(OnSceneLoaded);
    }

    private void OnSceneLoaded(string entryId)
    {
        GameObject spawnPoint = GameObject.Find(entryId);
        if(spawnPoint)
        {
            playerInstance.GetComponentInChildren<PlayerMovement>().OnAfterSpawn(spawnPoint.transform.position, spawnPoint.transform.rotation);
        } else
        {
            Debug.LogError("Spawn point missing! Please add one.");
        }
    }

    private void OnDestroy()
    {
        SceneLoader.Instance.onSceneChanged.RemoveListener(OnSceneLoaded);
    }
}
