using UnityEngine;

[CreateAssetMenu(menuName = "Effects/HealthBuff", order = 0)]
public class HealthBuff : Effect
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerMovement>().health += amount;
    }
}
