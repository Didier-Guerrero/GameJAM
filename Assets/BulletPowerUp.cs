using UnityEngine;

public class BulletPowerUp : MonoBehaviour
{
    public float speed = 20f; // Velocidad de la bala
    public float lifeTime = 2f; // Tiempo de vida de la bala antes de desaparecer

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed; // Mover la bala hacia adelante
        Destroy(gameObject, lifeTime); // Destruir la bala después de un tiempo
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destruir la bala al impactar con un enemigo
            Destroy(gameObject);
        }
    }
}
