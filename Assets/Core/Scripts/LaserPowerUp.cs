using UnityEngine;

public class LaserPowerUp : MonoBehaviour
{
    public int shots = 2; // количество выстрелов, которое даёт

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPowerUp pu = other.GetComponent<PlayerPowerUp>();
        if (pu != null)
        {
            pu.AddLaser(shots);
            Destroy(gameObject);
        }
    }
}