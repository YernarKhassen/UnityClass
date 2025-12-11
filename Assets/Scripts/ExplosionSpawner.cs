using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{
    public int radius = 3;
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
        obj.GetComponent<ExplosionSegment>().explosionID = explosionID;
    }

    void SpawnDirection(Vector2Int dir, GameObject middlePrefab, GameObject endPrefab)
    {
        Vector3 basePos = transform.position;

        for (int i = 1; i < radius; i++)
        {
            Vector3 pos = basePos + new Vector3(dir.x, dir.y) * i;
            var middle = Instantiate(middlePrefab, pos, Quaternion.identity);
            middle.GetComponent<ExplosionSegment>().explosionID = explosionID;
        }

        Vector3 endPos = basePos + new Vector3(dir.x, dir.y) * radius;
        var end = Instantiate(endPrefab, endPos, Quaternion.identity);
        end.GetComponent<ExplosionSegment>().explosionID = explosionID;
    }
}