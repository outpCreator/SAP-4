using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    // Player
    public GameObject playerPrefab;
    GameObject playerInstance;

    [Header("Public Referenzes")]
    // Player Movement
    public PlayerMovement playerMovementScript;
    // Player Comrainer
    public Transform playerContainer;
    // Player Avatar
    public Transform playerTransform;
    // Camera
    public Transform camTransform;
    // Camera Movement
    public CameraMovement cameraMovement;
    // Camera Position
    public Vector3 initialCameraPosition {  get; private set; }
    // Camera Rotation
    public Quaternion initialCameraRotation { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void InitPlayer()
    {
        // Player Instance
        playerInstance = Instantiate(playerPrefab);

        // Player Components
        playerMovementScript = playerInstance.GetComponentInChildren<PlayerMovement>();
        playerContainer = playerInstance.transform;
        playerTransform = playerMovementScript.transform;

        // Camera Components
        camTransform = Camera.main.transform;
        cameraMovement = playerTransform.GetComponent<CameraMovement>();

        // Camera Transforms
        initialCameraPosition = camTransform.localPosition;
        initialCameraRotation = camTransform.localRotation;

        // OnSceneChanged event
        SceneLoader.Instance.onSceneChanged.AddListener(OnSceneLoaded);


        cameraMovement.SetUpCamera();
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

        if (spawnPoint == null)
        {
            spawnPoint = GameObject.Find("SpawnPoint");
        }

        if (spawnPoint != null)
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
