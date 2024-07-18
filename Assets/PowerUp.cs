using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        TripleShot,
        PowerUpShot
    }

    public PowerUpType powerUpType;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            JetpackController playerController = other.GetComponent<JetpackController>();
            if (playerController != null)
            {
                if (powerUpType == PowerUpType.TripleShot)
                {
                    playerController.EnableTripleShot();
                }
                else if (powerUpType == PowerUpType.PowerUpShot)
                {
                    playerController.EnablePowerUpShot();
                }
                Destroy(gameObject); // Destruir el power-up después de recogerlo
            }
            else
            {
                Debug.LogError("No se encontró el JetpackController en el objeto del jugador.");
            }
        }
    }
}
