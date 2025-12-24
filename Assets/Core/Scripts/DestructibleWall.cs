using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    [Header("Drop Items")]
    public GameObject[] powerUps; // префабы улучшений
    [Range(0f, 1f)]
    public float dropChance = 0.3f;

    [Header("Wall Settings")]
    public GameObject destroyedEffect;

    public void DestroyWall(int explosionID)
    {
        // Визуальный эффект разрушения
        if (destroyedEffect != null)
            Instantiate(destroyedEffect, transform.position, Quaternion.identity);

        // Шанс выпадения PowerUp
        if (powerUps.Length > 0 && Random.value <= dropChance)
        {
            int index = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[index], transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}