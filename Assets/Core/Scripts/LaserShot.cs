using UnityEngine;

public class LaserShot : MonoBehaviour
{
    public float cellSize = 1f;
    public int maxDistance = 2; // макс. дальность до стены
    public int damage = 1;
    public LayerMask wallLayer; // слой стен
    public LayerMask playerLayer; // слой игрока

    public void Shoot(Vector2 direction, Vector3 startPos)
    {
        Vector3 pos = startPos;

        for (int i = 1; i <= maxDistance; i++)
        {
            pos += new Vector3(direction.x, direction.y) * cellSize;

            // Проверка на игрока
            Collider2D playerHit = Physics2D.OverlapPoint(pos, playerLayer);
            if (playerHit != null)
            {
                PlayerHealth ph = playerHit.GetComponent<PlayerHealth>();
                if (ph != null)
                    ph.TakeDamage(damage, 0);
            }

            // Проверка на стену
            Collider2D wallHit = Physics2D.OverlapPoint(pos, wallLayer);
            if (wallHit != null)
            {
                DestructibleWall wall = wallHit.GetComponent<DestructibleWall>();
                if (wall != null)
                    wall.DestroyWall(0);

                // лазер останавливается на стене
                break;
            }

            // Можно визуализировать лазер сегментом
            Debug.DrawLine(pos - new Vector3(direction.x, direction.y) * cellSize, pos, Color.red, 1f);
        }
    }
}