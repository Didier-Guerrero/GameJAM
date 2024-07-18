using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float parallaxEffectMultiplier = 0.5f; // Multiplicador del efecto parallax
    private Vector3 previousPlayerPosition;
    private Renderer backgroundRenderer;

    void Start()
    {
        previousPlayerPosition = player.position;
        backgroundRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Vector3 deltaMovement = player.position - previousPlayerPosition;
        Vector2 offset = backgroundRenderer.material.mainTextureOffset;
        offset.x += deltaMovement.x * parallaxEffectMultiplier / backgroundRenderer.material.mainTextureScale.x;
        backgroundRenderer.material.mainTextureOffset = offset;
        previousPlayerPosition = player.position;
    }
}
