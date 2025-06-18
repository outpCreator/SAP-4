using UnityEngine;
using UnityEngine.InputSystem;

public class Truhe : MonoBehaviour
{
    public int ressourceAmount = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement movement = other.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                print(movement + "is not null");
                // if(Player Input) =>{}
                if (movement.interaction)
                {
                    print("Interact");
                    // Play Animation

                    // Play Sound

                    // Give Player Ressource
                    movement.potionIngredience += ressourceAmount;

                    // Delete Chest
                    Destroy(gameObject);
                }
            }
        }
    }

    
}
