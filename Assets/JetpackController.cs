using UnityEngine;
using UnityEngine.UI;

public class JetpackController : MonoBehaviour
{
    public float jetpackForce = 10f;
    public float forwardSpeed = 5f;
    public float fuel = 100f;
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 1f;
    public float boostFuelConsumptionRate = 5f;
    public Text fuelText;
    public float rotationSpeed = 5f;
    public float heightThreshold = -5f;
    public float destroyHeight = 4.95f;
    public Sprite normalSprite;
    public Sprite lowHeightSprite;
    public Sprite powerUpSprite;
    public int playerHealth = 3;
    public Text healthText;
    public GameObject bulletPrefab;
    public GameObject powerUpBulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private bool tripleShotEnabled = false;
    private bool powerUpEnabled = false;
    private float nextFire = 0.0f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isFrozen = false;
    private bool isDestroyed = false;

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
            return;
        }

        if (!isFrozen)
        {
            if (fuel > 0)
            {
                fuel -= fuelConsumptionRate * Time.deltaTime;

                if (Input.GetKey(KeyCode.Space))
                {
                    rb.velocity = new Vector2(forwardSpeed, jetpackForce);
                    fuel -= boostFuelConsumptionRate * Time.deltaTime;
                }
                else
                {
                    rb.velocity = new Vector2(forwardSpeed, rb.velocity.y);
                }

                float targetRotationZ = 0;
                if (rb.velocity.y > 0)
                {
                    targetRotationZ = 30;
                }
                else if (rb.velocity.y < 0)
                {
                    targetRotationZ = -30;
                }

                Quaternion targetRotation = Quaternion.Euler(0, 0, targetRotationZ);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
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
                // Cambiar el sprite si el power-up está habilitado
                if (powerUpEnabled || tripleShotEnabled)
                {
                    spriteRenderer.sprite = powerUpSprite;
                }
                else
                {
                    spriteRenderer.sprite = normalSprite;
                }
            }

            UpdateFuelUI();

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
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    void TakeDamage()
    {
        playerHealth--;
        Debug.Log("Player Health: " + playerHealth);
        UpdateHealthUI();

        if (playerHealth <= 0)
        {
            isDestroyed = true;
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
        tripleShotEnabled = true;
        powerUpEnabled = false; // Deshabilitar otro power-up
        Debug.Log("Triple Shot habilitado.");
        spriteRenderer.sprite = powerUpSprite;
    }

    public void EnablePowerUpShot()
    {
        powerUpEnabled = true;
        tripleShotEnabled = false; // Deshabilitar disparo triple
        Debug.Log("Power-up habilitado, ahora disparando balas de power-up.");
        spriteRenderer.sprite = powerUpSprite;
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
