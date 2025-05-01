using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject playerPrefab;
    GameObject playerInstance;

    public PlayerMovement playerMovementScript;
    public Transform playerContainer;

    // Camera Position & Rotation
    public Transform camTransform;
    public Vector3 initialCameraPosition {  get; private set; }
    public Quaternion initialCameraRotation { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void InitPlayer()
    {
        playerInstance = Instantiate(playerPrefab);

        playerMovementScript = playerInstance.GetComponentInChildren<PlayerMovement>();
        playerContainer = playerInstance.transform;

        camTransform = playerContainer.GetComponentInChildren<CameraMovement>().transform;
        initialCameraPosition = camTransform.localPosition;
        initialCameraRotation = camTransform.localRotation;

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

    public IEnumerator HandleRoomTransition(Vector3 targetPosition, Quaternion targetRotation)
    {
        PlayerMovement playerMovement = playerInstance.GetComponentInChildren<PlayerMovement>();
        Transform playerContainer = playerInstance.transform;

        Transform playerTransform = playerMovement.transform;
        Transform originalParent = playerTransform.parent;
        playerTransform.parent = null;

        float duration = 0.4f;
        float elapsed = 0f;
        Vector3 startPos = playerContainer.position;
        Quaternion startRot = playerContainer.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t);
            playerContainer.position = Vector3.Lerp(startPos, targetPosition, t);
            playerContainer.rotation = startRot;
            yield return null;
        }

        playerContainer.position = targetPosition;

        playerTransform.parent = originalParent;

        yield return new WaitForSeconds(0.1f);
        playerMovement.FreezeMovement(false, targetPosition, targetRotation);
    }

    public void OnSceneLoaded(string entryId)
    {
        GameObject spawnPoint = GameObject.Find(entryId);

        if (spawnPoint == null)
        {
            spawnPoint = GameObject.Find("SpawnPoint");
        }

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
