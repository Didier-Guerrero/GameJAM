using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // El objeto que la c�mara seguir�
    public Vector3 offset; // Desplazamiento de la c�mara respecto al objeto

    void LateUpdate()
    {
        if (target != null)
        {
            // Obtener la posici�n actual de la c�mara
            Vector3 newPosition = transform.position;

            // Actualizar solo la posici�n en X de la c�mara
            newPosition.x = target.position.x + offset.x;

            // Mantener la posici�n en Y y Z original de la c�mara
            newPosition.y = transform.position.y;
            newPosition.z = target.position.z + offset.z;

            // Asignar la nueva posici�n a la c�mara
            transform.position = newPosition;

            // Mantener la rotaci�n original de la c�mara
            transform.rotation = Quaternion.identity;
        }
    }
}
