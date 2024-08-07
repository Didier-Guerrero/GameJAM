using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f; // Velocidad de la bala
    public float lifeTime = 2f; // Tiempo de vida de la bala antes de desaparecer

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed; // Mover la bala hacia adelante
        Destroy(gameObject, lifeTime); // Destruir la bala despu�s de un tiempo
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Bullet2"))
        {
            // Destruir la bala al impactar con un enemigo
            Destroy(gameObject);
        }
    }
}
