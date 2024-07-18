using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // El objeto que la cámara seguirá
    public Vector3 offset; // Desplazamiento de la cámara respecto al objeto

    void LateUpdate()
    {
        if (target != null)
        {
            // Obtener la posición actual de la cámara
            Vector3 newPosition = transform.position;

            // Actualizar solo la posición en X de la cámara
            newPosition.x = target.position.x + offset.x;

            // Mantener la posición en Y y Z original de la cámara
            newPosition.y = transform.position.y;
            newPosition.z = target.position.z + offset.z;

            // Asignar la nueva posición a la cámara
            transform.position = newPosition;

            // Mantener la rotación original de la cámara
            transform.rotation = Quaternion.identity;
        }
    }
}
