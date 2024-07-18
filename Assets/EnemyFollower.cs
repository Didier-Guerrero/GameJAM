using UnityEngine;
using System.Collections;

public class EnemyFollower : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform firePoint; // Punto desde donde se dispara la bala
    public float speed = 2f; // Velocidad del enemigo
    public float fireCooldown = 2f; // Tiempo de enfriamiento entre disparos
    public Sprite normalSprite; // Sprite normal del enemigo
    public Sprite damageSprite; // Sprite del enemigo dañado
    public float damageDuration = 0.5f; // Duración del sprite de daño
    public int maxHealth = 3; // Vida máxima del enemigo

    private SpriteRenderer spriteRenderer;
    private float fireTimer = 0f;
    private int currentHealth;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite; // Asegurarse de que el sprite normal esté configurado inicialmente
        currentHealth = maxHealth; // Inicializar la vida actual al máximo
    }

    void Update()
    {
        if (player == null)
        {
            //Debug.LogError("Player no está asignado en el inspector.");
            return;
        }

        // Seguir la posición y del jugador
        Vector3 targetPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        //Debug.Log($"Enemigo moviéndose hacia {targetPosition}");

        // Disparar cuando el enemigo esté a la misma altura que el jugador
        if (Mathf.Abs(transform.position.y - player.position.y) < 0.1f)
        {
            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireCooldown;
            }
        }

        // Reducir el temporizador de enfriamiento
        if (fireTimer > 0f)
        {
            fireTimer -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (firePoint != null && bulletPrefab != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Debug.Log("Disparando bala");
        }
        else
        {
           // Debug.LogError("FirePoint o BulletPrefab no está asignado en el inspector.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destruir el objeto cuando la vida llegue a cero
        }
        else
        {
            StartCoroutine(HandleDamage());
        }
    }

    IEnumerator HandleDamage()
    {
        // Cambiar al sprite de daño
        spriteRenderer.sprite = damageSprite;

        // Esperar un momento
        yield return new WaitForSeconds(damageDuration);

        // Volver al sprite normal
        spriteRenderer.sprite = normalSprite;
    }
}
