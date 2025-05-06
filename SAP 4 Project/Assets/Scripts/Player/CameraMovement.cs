using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [Header("Settings")]
    public float normalCamSpeed;
    public float combatCamSpeed;
    public float transitionSpeed;
    public float smoothTime = 0.6f;

    [Header("Offsets")]
    public Vector3 defaultOffset = Vector3.zero;
    public Vector3 combatOffset = new Vector3(0, 6, -6);

    Vector3 smoothPivotPosition;
    Vector3 pivotVelocity = Vector3.zero;

    Vector3 offsetVelocity = Vector3.zero;
    Vector3 currentOffset;
    Vector3 targetOffset;

    bool transitioningBack = false;


    [Header("Transforms")]
    public Transform playerContainer;
    public Transform cam;

    float camYaw = 0f;
    bool inCombat = false;

    Vector2 camInput = Vector2.zero;

    public void OnCamMove(InputAction.CallbackContext context)
    {
        camInput = context.ReadValue<Vector2>();
    }

    public void SetUpCamera()
    {
        if (cam == null) cam = PlayerManager.Instance.camTransform;
        if (playerContainer == null) playerContainer = PlayerManager.Instance.playerContainer;

        defaultOffset = PlayerManager.Instance.initialCameraPosition;

        currentOffset = defaultOffset;
        targetOffset = defaultOffset;

        camYaw = 0f;
    }

    void Update()
    {
        float rotationSpeed = inCombat ? combatCamSpeed : normalCamSpeed;
        camYaw += camInput.x * rotationSpeed * Time.deltaTime;

        currentOffset = Vector3.SmoothDamp(currentOffset, targetOffset, ref offsetVelocity, smoothTime);
        Quaternion rotation = Quaternion.Euler(0, camYaw, 0);
        Vector3 rotatedOffset = rotation * currentOffset;

        Vector3 pivotTarget;

        if (inCombat)
        {
            pivotTarget = transform.position;
        }
        else if (transitioningBack)
        {
            smoothPivotPosition = Vector3.SmoothDamp(smoothPivotPosition, playerContainer.position, ref pivotVelocity, smoothTime);

            if (Vector3.Distance(smoothPivotPosition, playerContainer.position) < 0.05f)
            {
                transitioningBack = false;
                smoothPivotPosition = playerContainer.position;
            }

            pivotTarget = smoothPivotPosition;
        }
        else
        {
            pivotTarget = playerContainer.position;
        }

        cam.position = pivotTarget + rotatedOffset;
        cam.LookAt(pivotTarget + Vector3.up * 1.5f);
    }

    public void SwitchCameraMode(bool active)
    {
        if (inCombat && !active)
        {
            transitioningBack = true;
            smoothPivotPosition = cam.position - currentOffset;
        }

        inCombat = active;

        targetOffset = active ? combatOffset : defaultOffset;
    }
}
