using UnityEngine;

public class FixedTurret : MonoBehaviour
{
    public Transform ship; // Referencia a la nave
    public Vector3 offset; // Offset desde la posición de la nave

    void Update()
    {
        // Mantener la posición de la torreta en la posición de la nave con un offset
        transform.position = ship.position + offset;

        // Mantener la rotación fija hacia adelante
        transform.rotation = Quaternion.identity;
    }
}
