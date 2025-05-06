using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100;
    [SerializeField] float baseDamage = 5f;
    [SerializeField] float attackRange = 0f;
    [SerializeField] float attackSpeed = 0f;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    public Transform cameraTransform;

    bool isFrozen = false;
    bool canMove = true;
    Vector3 moveDirection = Vector3.zero;

    // Components
    CharacterController charController;

    [Header("Fighting")]
    public float baseCooldown = 5f;
    public float minCooldown = 3f;

    [Header("Potion")]

    [Header("Brewing")]
    

    // Inpus
    Vector2 moveInput = Vector2.zero;
    bool potionInput = false;
    bool brewingInput = false;
    bool fixateRoomInput = false;

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

    public void OnFixateRoom(InputAction.CallbackContext context)
    {
        fixateRoomInput = context.action.triggered;
    }

    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isFrozen || canMove)
        {
            Move();
            FixateRoom();
        }
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

    public void FreezeMovement(bool freeze, Vector3 position, Quaternion rotation)
    {
        isFrozen = freeze;
        if (freeze && charController != null)
        {
            canMove = false;
            charController.velocity.Set(0, 0, 0);
            charController.enabled = false;
            transform.position = position;
            transform.rotation = rotation;
            charController.enabled = true;
        }
    }



    void FixateRoom()
    {
        if (fixateRoomInput)
        {
            int amount = 1;
            PlayerUIManager.Instance.SetFixedRoomCount(amount);
        }
    }

    public void OnAfterSpawn(Vector3 position, Quaternion rotation)
    {
        charController.enabled = false;
        transform.position = position;
        transform.rotation = rotation;
        charController.enabled = true;
    }
}
