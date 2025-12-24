using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bombPrefab;

    private Rigidbody2D rb;
    private Vector2 movement;

    // 🔥 LASER SYSTEM
    public PlayerPowerUp powerUp;
    public LaserShot laser;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // ========== INPUT SYSTEM ==========
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnBomb(InputAction.CallbackContext context)
    {
        if (context.performed)
            PlaceBomb();
    }

    public void OnLaserShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
            TryLaserShoot();
    }

    // ========== MOVEMENT ==========
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // ========== BOMB ==========
    private IEnumerator EnableCollisionAfterDelay(Collider2D player, Collider2D bomb, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (player != null && bomb != null)
            Physics2D.IgnoreCollision(player, bomb, false);
    }

    void PlaceBomb()
    {
        Vector3 gridPos = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            0
        );

        GameObject bomb = Instantiate(bombPrefab, gridPos, Quaternion.identity);

        Collider2D playerCollider = GetComponent<Collider2D>();
        Collider2D bombCollider = bomb.GetComponent<Collider2D>();

        if (playerCollider != null && bombCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, bombCollider, true);
        }

        StartCoroutine(EnableCollisionAfterDelay(playerCollider, bombCollider, 0.5f));
    }

    // ================= LASER SYSTEM =================
    private void TryLaserShoot()
    {
        if (!powerUp.HasLaser)
        {
            Debug.Log("NO LASER SHOTS LEFT!");
            return;
        }

        Vector2 direction = Vector2.up; // позже сделаем по направлению движения

        laser.Shoot(direction, transform.position);
        powerUp.UseLaser();

        Debug.Log("Laser Fired! Remaining: " + powerUp.laserShots);
    }
}
