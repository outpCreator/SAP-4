using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ArmyUnit : MonoBehaviour
{
    [Header("Movement")]
    public float followSpeed;
    public float rotateSpeed = 2f;
    public float followDistance;
    public float followDelay = 0.2f;
    public int historyLength = 20;

    Queue<Vector3> positionHistory = new Queue<Vector3>();
    public Transform followTarget;

    [Header("GroundChecks")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float heightOffset;

    ArmyController controller;

    public void Initialize(ArmyController armyController, Transform target = null)
    {
        this.controller = armyController;
        this.followTarget = target;
    }

    private void Update()
    {
        Movement();
        PositionHistory();
    }

    void Movement()
    {
        Vector3 target;

        if (followTarget == null)
        {
            target = controller.spawnPoint.position;
        }
        else
        {
            var history = GetPositionHistory();
            if (history.Count < 2 || history == null) return;

            float currentDistance = 0f;
            target = transform.position;

            Vector3[] points = history.ToArray();
            for (int i = points.Length - 1; i > 0; i--)
            {
                Vector3 point1 = points[i];
                Vector3 point2 = points[i - 1];
                float segment = Vector3.Distance(point1, point2);

                currentDistance += segment;

                if (currentDistance >= followSpeed)
                {
                    float overshoot = currentDistance - followDistance;
                    float t = Mathf.Clamp01(1f - overshoot / segment);
                    target = Vector3.Lerp(point1, point2, t);
                    break;
                }
            }
        }

        if (followTarget != null && Vector3.Distance(transform.position, target) < followDistance) return;

        if (groundCheck)
        {
            Vector3 rayOrigin = groundCheck.position + Vector3.down * 0.1f;
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
            {
                target.y = hit.point.y + heightOffset;
            }
        }

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * followSpeed);

        if (followTarget != null)
        {
            Vector3 direction = target - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
        }
    }

    void PositionHistory()
    {
        positionHistory.Enqueue(transform.position);

        int maxLenght = Mathf.CeilToInt((followDistance * 10) / Time.fixedDeltaTime);

        while (positionHistory.Count > maxLenght)
        {
            positionHistory.Dequeue();
        }
    }

    Queue<Vector3> GetPositionHistory()
    {
        if (followTarget != null && followTarget.TryGetComponent(out ArmyUnit unit))
        {
            return unit.positionHistory;
        }

        return null;
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
}
