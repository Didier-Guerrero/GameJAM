using UnityEngine;

public class PowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            JetpackController playerController = other.GetComponent<JetpackController>();
            if (playerController != null)
            {
                playerController.EnableTripleShot();
                Destroy(gameObject); // Destruir el power-up después de recogerlo
            }
        }
    }
}
