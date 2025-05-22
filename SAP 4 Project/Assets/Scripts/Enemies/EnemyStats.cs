using UnityEngine;

[CreateAssetMenu(fileName = ("EnemyStats"))]
public class EnemyStats : ScriptableObject
{
    [Header("Stats")]
    public float moveSpeed = 0;
    public float idleMoveRange = 0;

    public float health = 0;

    public float attackDamage = 0;
    public float attackCooldown = 0;
    public float detectionRange = 0;
    public float attackRange = 0;
}
