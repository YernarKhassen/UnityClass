using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    public int laserShots = 0; // текущие доступные выстрелы

    public bool HasLaser => laserShots > 0;

    public void UseLaser()
    {
        if (laserShots > 0)
            laserShots--;
    }

    public void AddLaser(int amount)
    {
        laserShots += amount;
        Debug.Log($"Игрок получил {amount} выстрелов лазера! Всего: {laserShots}");
    }
}