using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ArmyUnit : MonoBehaviour
{
    [Header("Movement")]
    public ArmyUnit leader;
    public float followSpeed;
    public float followDistance;
    public int historyLength = 20;

    Queue<Vector3> positionHistory = new Queue<Vector3>();
    Vector3 currentTarget;

    [Header("GroundChecks")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float heightOffset;

    ArmyController controller;

    public void Initialize(ArmyController armyController)
    {
        controller = armyController;

        
    }

    private void Update()
    {
        
    }

    public void SetScale(Vector3 targetScale, float duration)
    {
        StartCoroutine(Scale(targetScale, duration));
    }

    IEnumerator Scale(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time /  duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private void LateUpdate()
    {
        SetGroundPosition();
    }

    void SetGroundPosition()
    {
        if (!groundCheck) return;

        Vector3 origin = groundCheck.position + Vector3.up * -0.1f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            Vector3 pos = transform.position;
            float targetY = hit.point.y + heightOffset;

            pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * 10f);
            transform.position = pos;
        }
    }
}
