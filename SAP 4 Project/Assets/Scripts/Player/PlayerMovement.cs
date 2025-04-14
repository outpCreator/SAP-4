using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;

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
        Move();

        if (potionInput) Potion();

        if (brewingInput) Brewing();
    }

    void Move()
    {
        float moveX = moveInput.x;
        float moveZ = moveInput.y;

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ) * moveSpeed;

        charController.SimpleMove(moveDirection);
    }

    void Potion()
    {

    }

    void Brewing()
    {

    }
}
