using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [Header("Explosion Prefabs")]
    public GameObject explosionSpawnerPrefab;

    [Header("Fuse Settings")]
    public float fuseTime = 3f;

    [Header("Explosion Scaling")]
    [Tooltip("Multiplier for explosion radius and visual scale.")]
    [Min(0.1f)] public float explosionScale = 1f;

    private void Start()
    {
        StartCoroutine(ExplodeAfterDelay(fuseTime));
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (explosionSpawnerPrefab != null)
        {
            var spawnerObj = Instantiate(explosionSpawnerPrefab, transform.position, Quaternion.identity);
            var spawner = spawnerObj.GetComponent<ExplosionSpawner>();
            if (spawner != null)
            {
                spawner.explosionScale = explosionScale;
            }
            else
            {
                // Fallback: scale transform if component missing
                spawnerObj.transform.localScale *= explosionScale;
            }
        }
        else
        {
            Debug.LogError("Bomb: No prefabs assigned (explosionPrefab or explosionSpawnerPrefab)!");
        }

        Destroy(gameObject);
    }
}