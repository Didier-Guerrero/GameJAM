using UnityEngine;
using System.Collections;

public class EnemyFollower2 : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform firePoint; // Punto desde donde se dispara la bala
    public float speed = 2f; // Velocidad del enemigo
    public float fireCooldown = 2f; // Tiempo de enfriamiento entre disparos
    public Sprite normalSprite; // Sprite normal del enemigo
    public Sprite damageSprite; // Sprite del enemigo da�ado
    public float damageDuration = 0.5f; // Duraci�n del sprite de da�o
    public int maxHealth = 3; // Vida m�xima del enemigo
    public GameObject powerUpPrefab; // Prefab del power-up que deja al morir
    public float detectionRange = 5f; // Rango de detecci�n del jugador

    private SpriteRenderer spriteRenderer;
    private float fireTimer = 0f;
    private int currentHealth;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = normalSprite; // Asegurarse de que el sprite normal est� configurado inicialmente
        currentHealth = maxHealth; // Inicializar la vida actual al m�ximo
        rb.gravityScale = 0; // Asegurarse de que la gravedad no afecta al enemigo
        rb.freezeRotation = true; // Evitar que el enemigo gire al colisionar
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player no est� asignado en el inspector.");
            return;
        }

        if (currentHealth > 0)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Seguir la posici�n del jugador
                Vector3 targetPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                Debug.Log($"Enemigo movi�ndose hacia {targetPosition}, posici�n actual {transform.position}");

                // Disparar cuando el enemigo est� a la misma altura que el jugador
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
        }
    }

    void Shoot()
    {
        if (firePoint != null && bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, -firePoint.position, Quaternion.identity);
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Debug.Log("Disparando bala");
        }
        else
        {
            Debug.LogError("FirePoint o BulletPrefab no est� asignado en el inspector.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
        }
        else if (collision.gameObject.CompareTag("BalaPower"))
        {
            TakeDamagePower();
        }
    }

    void TakeDamagePower()
    {
        currentHealth -= 2;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HandleDamage());
        }
    }

    void TakeDamage()
    {
        currentHealth--;
        Debug.Log("Vida del enemigo: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HandleDamage());
        }
    }

    void Die()
    {
        // Instanciar el power-up
        if (powerUpPrefab != null)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            Debug.Log("Instanciando power-up.");
        }
        else
        {
            Debug.LogError("PowerUpPrefab no est� asignado en el inspector.");
        }

        Debug.Log("Enemigo destruido y power-up instanciado.");
        Destroy(gameObject); // Destruir el objeto enemigo
        Debug.Log("Objeto enemigo destruido.");
    }

    IEnumerator HandleDamage()
    {
        // Cambiar al sprite de da�o
        spriteRenderer.sprite = damageSprite;

        // Esperar un momento
        yield return new WaitForSeconds(damageDuration);

        // Volver al sprite normal
        spriteRenderer.sprite = normalSprite;
    }
}
