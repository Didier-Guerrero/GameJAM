using UnityEngine;
using UnityEngine.UI;

public class JetpackController : MonoBehaviour
{
    public float jetpackForce = 10f; // Fuerza del jetpack
    public float forwardSpeed = 5f; // Velocidad hacia adelante
    public float fuel = 100f; // Cantidad inicial de gasolina
    public float fuelConsumptionRate = 1f; // Tasa de consumo de gasolina por segundo
    public float boostFuelConsumptionRate = 5f; // Tasa de consumo de gasolina cuando se asciende
    public Text fuelText; // UI Text para mostrar la gasolina restante
    public float rotationSpeed = 5f; // Velocidad de rotación
    public float heightThreshold = -5f; // Umbral de altura
    public float destroyHeight = 4.95f; // Altura a la que se destruye la nave
    public Sprite normalSprite; // Sprite normal
    public Sprite lowHeightSprite; // Sprite cuando está por debajo del umbral
    public int playerHealth = 3; // Vida del jugador
    public Text healthText; // UI Text para mostrar la vida del jugador

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
                spriteRenderer.sprite = normalSprite;
            }

            UpdateFuelUI();
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
}
