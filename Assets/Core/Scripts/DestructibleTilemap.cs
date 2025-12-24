using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles per-cell destruction for a Tilemap (e.g., breakable walls).
/// Attach to the Tilemap GameObject that has a TilemapCollider2D.
/// </summary>
[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapCollider2D))]
public class DestructibleTilemap : MonoBehaviour
{
    [Header("Drops / Effects")]
    public GameObject[] powerUps;
    [Range(0f, 1f)]
    public float dropChance = 0.3f;
    public GameObject destroyedEffect;

    [Header("Damage Settings")]
    [Tooltip("Hits required to destroy a tile. 1 = destroy immediately.")]
    [Min(1)] public int hitsToDestroy = 1;
    [Tooltip("Optional tile to replace on first hit (e.g., cracked version).")]
    public TileBase damagedTile;

    private Tilemap tilemap;
    private readonly HashSet<Vector3Int> destroyedCells = new HashSet<Vector3Int>();
    private readonly Dictionary<Vector3Int, int> hitCounts = new Dictionary<Vector3Int, int>();

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    /// <summary>
    /// Destroys the tile at the given world position if present.
    /// </summary>
    public void DestroyTileAt(Vector3 worldPos, int explosionID)
    {
        if (tilemap == null) return;

        Vector3Int cell = tilemap.WorldToCell(worldPos);
        if (!tilemap.HasTile(cell)) return;

        ProcessHit(cell);
    }

    /// <summary>
    /// Destroys all tiles whose cells intersect the provided world-space bounds.
    /// Useful when an explosion collider overlaps multiple tiles.
    /// </summary>
    public void DestroyTilesInBounds(Bounds worldBounds, int explosionID)
    {
        if (tilemap == null) return;

        // Slightly expand bounds to catch edge cases on borders
        const float epsilon = 0.05f;
        worldBounds.Expand(epsilon);

        Vector3 min = worldBounds.min;
        Vector3 max = worldBounds.max;

        Vector3Int minCell = tilemap.WorldToCell(min);
        Vector3Int maxCell = tilemap.WorldToCell(max);

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(cell)) continue;

                ProcessHit(cell);
            }
        }
    }

    private void ProcessHit(Vector3Int cell)
    {
        // already destroyed
        if (destroyedCells.Contains(cell)) return;

        int hits = 0;
        if (hitCounts.TryGetValue(cell, out var stored))
            hits = stored;
        hits++;
        hitCounts[cell] = hits;

        if (hits < hitsToDestroy)
        {
            // swap to damaged tile if provided
            if (damagedTile != null)
                tilemap.SetTile(cell, damagedTile);
            return;
        }

        // destroy
        destroyedCells.Add(cell);
        Vector3 spawnPos = tilemap.GetCellCenterWorld(cell);

        if (destroyedEffect != null)
            Instantiate(destroyedEffect, spawnPos, Quaternion.identity);

        if (powerUps.Length > 0 && Random.value <= dropChance)
        {
            int index = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[index], spawnPos, Quaternion.identity);
        }

        tilemap.SetTile(cell, null);
    }
}

