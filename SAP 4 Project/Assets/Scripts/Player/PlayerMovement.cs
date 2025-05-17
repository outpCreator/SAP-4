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

    bool canAttack = true;

    EnemyCombat currentTarget;
    int targetIndex = 0;

    [Header("Potion")]

    [Header("Brewing")]
    

    // Inpus
    Vector2 moveInput = Vector2.zero;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnPotion(InputAction.CallbackContext context)
    {
        if (context.performed && currentTarget != null && canAttack)
        {
            currentTarget.TakeDamage(baseDamage * 5);
            canAttack = false;

            StartCoroutine(Cooldown());

            Debug.Log("Angriff ausgeführt auf: " + currentTarget.name);
        }
    }
    public void OnNextTarget(InputAction.CallbackContext context)
    {
        if (context.performed && FightManager.Instance.activeEnemies.Count > 0)
        {
            EnemyCombat prevoiuseTarget = currentTarget;

            targetIndex = (targetIndex + 1) % FightManager.Instance.activeEnemies.Count;
            currentTarget = FightManager.Instance.activeEnemies[targetIndex];

            UpdateTargetHighlight(prevoiuseTarget, currentTarget);

            Debug.Log("Ziel gewechselt zu: " + currentTarget.name);
        }
    }
    public void OnPreviouseTarget(InputAction.CallbackContext context)
    {
        if (context.performed && FightManager.Instance.activeEnemies.Count > 0)
        {
            EnemyCombat prevoiuseTarget = currentTarget;

            targetIndex--;
            if (targetIndex < 0) targetIndex = FightManager.Instance.activeEnemies.Count - 1;
            currentTarget = FightManager.Instance.activeEnemies[targetIndex];

            UpdateTargetHighlight(prevoiuseTarget, currentTarget);

            Debug.Log("Ziel gewechselt zu: " + currentTarget.name);
        }
    }

    void UpdateTargetHighlight(EnemyCombat previous, EnemyCombat current)
    {
        
    }

    public void OnBrewing(InputAction.CallbackContext context)
    {

    }

    public void OnFixateRoom(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int amount = 1;
            PlayerUIManager.Instance.SetFixedRoomCount(amount);
        }
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

        if (currentTarget != null) //<-- target
        {
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            targetDirection.y = 0f;

            if(targetDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else if (moveDirection.magnitude > 0.1f) //<-- no target
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

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(baseCooldown);

        canAttack = true;
        print("You can attack!");
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    public void OnAfterSpawn(Vector3 position, Quaternion rotation)
    {
        charController.enabled = false;
        transform.position = position;
        transform.rotation = rotation;
        charController.enabled = true;
    }
}
