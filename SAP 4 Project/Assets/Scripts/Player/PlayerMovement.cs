using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    public Transform cameraTransform;

    bool isFrozen = false;
    Vector3 moveDirection = Vector3.zero;

    // Components
    CharacterController charController;
   

    [Header("Potion")]
    [SerializeField] float baseDamage;

    [Header("Brewing")]
    public 

    // Inpus
    Vector2 moveInput = Vector2.zero;
    bool potionInput = false;
    bool brewingInput = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnPotion(InputAction.CallbackContext context)
    {
        potionInput = context.action.triggered;
    }
    public void OnBrewing(InputAction.CallbackContext context)
    {
        brewingInput = context.action.triggered;
    }

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (isFrozen) return;

        Move();

        if (potionInput) Potion();

        if (brewingInput) Brewing();
    }

    void Move()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        charController.SimpleMove(moveDirection * moveSpeed);

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void FreezeMovement(bool freeze)
    {
        isFrozen = freeze;
        if (freeze && charController != null)
        {
            charController.velocity.Set(0, 0, 0);
        }
    }

    public void AlignMoveDirection(Quaternion containerRotation)
    {
        moveDirection = containerRotation * Vector3.forward;
        moveDirection.y = 0f;
        moveDirection.Normalize();
    }

    void Potion()
    {

    }

    void Brewing()
    {

    }

    public void OnAfterSpawn(Vector3 position, Quaternion rotation)
    {
        charController.enabled = false;
        transform.position = position;
        transform.rotation = rotation;
        charController.enabled = true;
    }
}
