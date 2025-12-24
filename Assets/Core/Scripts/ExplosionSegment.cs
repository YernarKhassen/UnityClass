using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class ExplosionSegment : MonoBehaviour
{
    public int damage = 1;
    public int explosionID;

    private void Start()
    {
        Destroy(gameObject, 0.35f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage, explosionID);
            Debug.Log("Hit player!");
        }

        DestructibleWall wall = other.GetComponent<DestructibleWall>();
        if (wall != null)
        {
            wall.DestroyWall(explosionID);
            Debug.Log("Hit wall!");
            return;
        }

        // Tilemap-based breakable walls (per-cell destruction)
        DestructibleTilemap destructibleTilemap = other.GetComponent<DestructibleTilemap>();
        if (destructibleTilemap != null)
        {
            // Use this explosion segment collider bounds, not the whole tilemap bounds
            var myCol = GetComponent<Collider2D>();
            if (myCol != null)
            {
                destructibleTilemap.DestroyTilesInBounds(myCol.bounds, explosionID);
                Debug.Log("Hit breakable tile(s) in range!");
            }
        }
    }
}