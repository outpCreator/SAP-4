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

    float damageMultiplier = 0f;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    public Transform cameraTransform;

    bool isFrozen = false;
    bool canMove = true;
    Vector3 moveDirection = Vector3.zero;

    // Components
    CharacterController charController;
   
    [Header("Potion")]

    [Header("Brewing")]
    

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
        if (!isFrozen || canMove)
        {
            Move();
            Potion();
            Brewing();
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

    public IEnumerator SmoothStep(float duration = 0.2f)
    {
        Vector3 initialDirection = moveDirection;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = 1f -(elapsed / duration);
            moveDirection = initialDirection * t;

            yield return null;
        }

        moveDirection = Vector3.zero;
    }

    void Potion()
    {
        EnemyStats targetEnemyStats = FindAnyObjectByType<EnemyStats>();

        float actualDamage = baseDamage + damageMultiplier;
        bool canAttack = true;

        if (potionInput)
        {
            if (canAttack && targetEnemyStats != null)
            {
                print("ATTACK!");
                canAttack = false;

                targetEnemyStats.health -= actualDamage;

                float duration = 0f;

                duration += Time.deltaTime;

                if (duration >= attackSpeed)
                {
                    canAttack = true;

                    print($"Attack done! Enemyhealth at {targetEnemyStats.health}, having dealed {actualDamage} damage!");
                }
            }
        }
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
