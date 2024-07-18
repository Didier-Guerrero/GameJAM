using UnityEngine;
using UnityEngine.UI;

public class JetpackController : MonoBehaviour
{
    public float jetpackForce = 10f; // Fuerza del jetpack
    public float forwardSpeed = 5f; // Velocidad hacia adelante
    public float fuel = 100f; // Cantidad inicial de gasolina
    public float maxFuel = 100f; // Cantidad máxima de gasolina
    public float fuelConsumptionRate = 1f; // Tasa de consumo de gasolina por segundo
    public float boostFuelConsumptionRate = 5f; // Tasa de consumo de gasolina cuando se asciende
    public Text fuelText; // UI Text para mostrar la gasolina restante
    public float rotationSpeed = 5f; // Velocidad de rotación
    public float heightThreshold = -5f; // Umbral de altura
    public float destroyHeight = 4.95f; // Altura a la que se destruye la nave
    public Sprite normalSprite; // Sprite normal
    public Sprite lowHeightSprite; // Sprite cuando está por debajo del umbral
    public Sprite powerUpSprite; // Sprite cuando el power-up está activo
    public int playerHealth = 3; // Vida del jugador
    public Text healthText; // UI Text para mostrar la vida del jugador
    public GameObject bulletPrefab; // Prefab de la bala normal
    public GameObject powerUpBulletPrefab; // Prefab de la bala del power-up
    public Transform firePoint; // Punto desde donde se dispara la bala
    public float fireRate = 0.5f; // Cadencia de disparo
    private bool tripleShotEnabled = false; // Indica si el disparo triple está habilitado
    private bool powerUpEnabled = false; // Indica si la bala del power-up está habilitada
    private float nextFire = 0.0f; // Tiempo para el próximo disparo

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isFrozen = false; // Estado de congelación
    private bool isDestroyed = false; // Estado de destrucción

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateFuelUI();
        UpdateHealthUI();
        if (firePoint == null)
        {
            Debug.LogError("FirePoint no está asignado en el inspector.");
        }
        else
        {
            Debug.Log("FirePoint asignado correctamente.");
        }
    }

    void Update()
    {
        if (isDestroyed)
        {
            return; // Si ya está destruido, salir de la función
        }

        if (!isFrozen)
        {
            if (fuel > 0)
            {
                // Consumo de gasolina por segundo
                fuel -= fuelConsumptionRate * Time.deltaTime;

                // Aplicar fuerza hacia arriba si se mantiene presionada la tecla de espacio y consumir más gasolina
                if (Input.GetKey(KeyCode.Space))
                {
                    rb.velocity = new Vector2(forwardSpeed, jetpackForce);
                    fuel -= boostFuelConsumptionRate * Time.deltaTime;
                }
                else
                {
                    // Mantener la velocidad hacia adelante
                    rb.velocity = new Vector2(forwardSpeed, rb.velocity.y);
                }

                // Cambiar la orientación del personaje basado en su velocidad vertical
                float targetRotationZ = 0;
                if (rb.velocity.y > 0) // Está subiendo
                {
                    targetRotationZ = 30; // Rotar hacia arriba
                }
                else if (rb.velocity.y < 0) // Está bajando
                {
                    targetRotationZ = -30; // Rotar hacia abajo
                }

                Quaternion targetRotation = Quaternion.Euler(0, 0, targetRotationZ);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // Si no hay gasolina, el personaje solo avanza hacia adelante
                rb.velocity = new Vector2(forwardSpeed, rb.velocity.y);
            }

            // Cambiar el sprite y estado basado en la altura
            if (transform.position.y < heightThreshold)
            {
                spriteRenderer.sprite = lowHeightSprite;
                FreezeCharacter();
            }
            else if (transform.position.y > destroyHeight)
            {
                spriteRenderer.sprite = lowHeightSprite;
                FreezeCharacter();
            }
            else
            {
                if (powerUpEnabled)
                {
                    spriteRenderer.sprite = powerUpSprite;
                }
                else
                {
                    spriteRenderer.sprite = normalSprite;
                }
            }

            UpdateFuelUI();

            // Manejar disparo
            if (Input.GetKey(KeyCode.F) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                if (tripleShotEnabled)
                {
                    FireTripleShot();
                }
                else
                {
                    FireSingleShot();
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            FreezeCharacter();
            spriteRenderer.sprite = lowHeightSprite;
        }
        else if (collision.gameObject.CompareTag("Bullet2"))
        {
            TakeDamage();
        }
    }

    void FreezeCharacter()
    {
        isFrozen = true;
        rb.velocity = Vector2.zero; // Detener el movimiento
        rb.isKinematic = true; // Desactivar la física
    }

    void TakeDamage()
    {
        playerHealth--;
        Debug.Log("Player Health: " + playerHealth);
        UpdateHealthUI();

        if (playerHealth <= 0)
        {
            isDestroyed = true;
            // Aquí puedes agregar lógica adicional cuando el jugador muere, como reiniciar el nivel
            FreezeCharacter();
            spriteRenderer.sprite = lowHeightSprite;
        }
    }

    void UpdateFuelUI()
    {
        if (fuelText != null)
        {
            fuelText.text = "Fuel: " + Mathf.Max(fuel, 0).ToString("F2");
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + playerHealth.ToString();
        }
    }

    public void EnableTripleShot()
    {
        spriteRenderer.sprite = powerUpSprite;
        tripleShotEnabled = true;

        // Opcionalmente, puedes agregar un temporizador para deshabilitar el triple disparo después de un tiempo
    }

    public void EnablePowerUpShot()
    {
        powerUpEnabled = true;
        Debug.Log("Power-up habilitado, ahora disparando balas de power-up.");
        spriteRenderer.sprite = powerUpSprite; // Cambiar al sprite del power-up
        // Opcionalmente, puedes agregar un temporizador para deshabilitar el disparo de power-up después de un tiempo
    }

    public void RestoreFuel()
    {
        fuel = maxFuel;
        UpdateFuelUI();
        Debug.Log("Gasolina restaurada al 100%.");
    }

    void FireSingleShot()
    {
        if (firePoint != null)
        {
            GameObject bullet;
            if (powerUpEnabled)
            {
                bullet = Instantiate(powerUpBulletPrefab, firePoint.position, Quaternion.identity);
                Debug.Log("Disparando bala de power-up.");
            }
            else
            {
                bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Debug.Log("Disparando bala normal.");
            }
        }
        else
        {
            Debug.LogError("FirePoint no está asignado en el inspector.");
        }
    }

    void FireTripleShot()
    {
        if (firePoint != null)
        {
            GameObject centralBullet, leftBullet, rightBullet;
            if (powerUpEnabled)
            {
                centralBullet = Instantiate(powerUpBulletPrefab, firePoint.position, Quaternion.identity);
                leftBullet = Instantiate(powerUpBulletPrefab, firePoint.position, Quaternion.Euler(0, 0, 15));
                rightBullet = Instantiate(powerUpBulletPrefab, firePoint.position, Quaternion.Euler(0, 0, -15));
                Debug.Log("Disparando triple bala de power-up.");
            }
            else
            {
                spriteRenderer.sprite = powerUpSprite;
                centralBullet = Instantiate(powerUpBulletPrefab, firePoint.position, Quaternion.identity);
                leftBullet = Instantiate(powerUpBulletPrefab, firePoint.position, Quaternion.Euler(0, 0, 15));
                rightBullet = Instantiate(powerUpBulletPrefab, firePoint.position, Quaternion.Euler(0, 0, -15));
                Debug.Log("Disparando triple bala normal.");
            }
        }
        else
        {
            Debug.LogError("FirePoint no está asignado en el inspector.");
        }
    }
}
