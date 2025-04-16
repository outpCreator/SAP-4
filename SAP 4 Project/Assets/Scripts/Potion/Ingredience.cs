using UnityEngine;

public class Ingredience : MonoBehaviour
{
    public Effect effect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            effect.Apply(other.gameObject);
            Destroy(gameObject);
        }
    }
}
