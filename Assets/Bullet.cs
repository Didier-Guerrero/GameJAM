using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Añade lógica de colisión aquí (destruir bala, aplicar daño, etc.)
        Destroy(gameObject);
    }
}
