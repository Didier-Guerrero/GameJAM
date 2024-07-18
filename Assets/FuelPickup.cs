using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            JetpackController playerController = other.GetComponent<JetpackController>();
            if (playerController != null)
            {
                playerController.RestoreFuel();
                Destroy(gameObject); // Destruir el prefab de la gasolina después de recogerlo
            }
        }
    }
}
