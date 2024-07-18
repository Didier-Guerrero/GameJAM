using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Sprite normalSprite; // Sprite normal del enemigo
    public Sprite damageSprite; // Sprite del enemigo dañado
    public float damageDuration = 0.5f; // Duración del sprite de daño
    public int maxHealth = 3; // Vida máxima del enemigo

    private SpriteRenderer spriteRenderer;
    private int currentHealth;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite; // Asegurarse de que el sprite normal esté configurado inicialmente
        currentHealth = maxHealth; // Inicializar la vida actual al máximo
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
