using UnityEngine;

public class PowerUpSafeDome : MonoBehaviour
{
    [SerializeField] private float invincibleDuration = 3f;
    [SerializeField] private GameObject domeVisual; // визуальный эффект (опционально)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerSafeDome dome = other.GetComponent<PlayerSafeDome>();

        if (dome == null)
            dome = other.gameObject.AddComponent<PlayerSafeDome>();

        dome.Activate(invincibleDuration);

        if (domeVisual != null)
            Instantiate(domeVisual, other.transform);

        Destroy(gameObject);
    }
}
