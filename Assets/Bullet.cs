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
        // A�ade l�gica de colisi�n aqu� (destruir bala, aplicar da�o, etc.)
        Destroy(gameObject);
    }
}
