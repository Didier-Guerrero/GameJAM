using UnityEngine;

public class FixedTurret : MonoBehaviour
{
    public Transform ship; // Referencia a la nave
    public Vector3 offset; // Offset desde la posici�n de la nave

    void Update()
    {
        // Mantener la posici�n de la torreta en la posici�n de la nave con un offset
        transform.position = ship.position + offset;

        // Mantener la rotaci�n fija hacia adelante
        transform.rotation = Quaternion.identity;
    }
}
