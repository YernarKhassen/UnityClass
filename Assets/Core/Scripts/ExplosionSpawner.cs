using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{
    public int radius = 3;
    [Tooltip("Multiplier applied to radius and spawned segment scale.")]
    [Min(0.1f)] public float explosionScale = 1f;

    public GameObject centerPrefab;
    public GameObject middlePrefab;
    public GameObject endUpPrefab;
    public GameObject endDownPrefab;
    public GameObject endLeftPrefab;
    public GameObject endRightPrefab;

    private static int nextExplosionID = 1;
    private int explosionID;

    private void Start()
    {
        explosionID = nextExplosionID++;
        SpawnCenter();
        SpawnDirection(Vector2Int.up, middlePrefab, endUpPrefab);
        SpawnDirection(Vector2Int.down, middlePrefab, endDownPrefab);
        SpawnDirection(Vector2Int.left, middlePrefab, endLeftPrefab);
        SpawnDirection(Vector2Int.right, middlePrefab, endRightPrefab);

        Destroy(gameObject, 0.1f);
    }

    void SpawnCenter()
    {
        var obj = Instantiate(centerPrefab, transform.position, Quaternion.identity);
        ApplyScale(obj.transform);
        obj.GetComponent<ExplosionSegment>().explosionID = explosionID;
    }

    void SpawnDirection(Vector2Int dir, GameObject middlePrefab, GameObject endPrefab)
    {
        Vector3 basePos = transform.position;

        int effectiveRadius = Mathf.Max(1, Mathf.RoundToInt(radius * explosionScale));

        for (int i = 1; i < effectiveRadius; i++)
        {
            Vector3 pos = basePos + new Vector3(dir.x, dir.y) * i;
            var middle = Instantiate(middlePrefab, pos, Quaternion.identity);
            ApplyScale(middle.transform);
            middle.GetComponent<ExplosionSegment>().explosionID = explosionID;
        }

        Vector3 endPos = basePos + new Vector3(dir.x, dir.y) * effectiveRadius;
        var end = Instantiate(endPrefab, endPos, Quaternion.identity);
        ApplyScale(end.transform);
        end.GetComponent<ExplosionSegment>().explosionID = explosionID;
    }

    private void ApplyScale(Transform t)
    {
        float s = Mathf.Max(0.1f, explosionScale);
        t.localScale *= s;
    }
}