using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 20f; // Velocidad de la bala
    public float lifeTime = 2f; // Tiempo de vida de la bala antes de desaparecer

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D no encontrado en el prefab de la bala.");
            return;
        }
    }

    void Start()
    {
        // Aplicar la velocidad en la dirección de la rotación inicial del prefab hacia la izquierda
        rb.velocity = -transform.right * speed; // Nota: usar -transform.up si es necesario

        // Destruir la bala después de un tiempo
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            // Destruir la bala al impactar con el jugador
            Destroy(gameObject);
        }
    }
}
