using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float camSpeed;
    public Transform cam;
    public Transform pivot;

    Vector2 camInput = Vector2.zero;

    public void OnCamMove(InputAction.CallbackContext context)
    {
        camInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        float camX = camInput.x;

        cam.RotateAround(pivot.position, Vector3.up, camX * camSpeed * Time.deltaTime);
    }
}
