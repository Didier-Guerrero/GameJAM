using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform firePoint; // Punto desde donde se dispara la bala

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Disparar con el botón de disparo (por defecto, el botón izquierdo del ratón)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
