using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [Header("Explosion Prefabs")]
    public GameObject explosionSpawnerPrefab;

    [Header("Fuse Settings")]
    public float fuseTime = 3f;

    private void Start()
    {
        StartCoroutine(ExplodeAfterDelay(fuseTime));
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (explosionSpawnerPrefab != null)
        {
            Instantiate(explosionSpawnerPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Bomb: No prefabs assigned (explosionPrefab or explosionSpawnerPrefab)!");
        }

        Destroy(gameObject);
    }
}