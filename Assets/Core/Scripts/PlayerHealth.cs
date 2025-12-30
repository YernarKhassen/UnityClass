using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public GameObject healthBarPrefab;

    private int currentHealth;
    private bool isDead = false;
    private HealthBar bar;

    private HashSet<int> explosionsHit = new HashSet<int>();

    private void Start()
    {
        currentHealth = maxHealth;

        PlayerManager playerManager = PlayerManager.Instance;
        if (playerManager != null)
        {
            playerManager.RegisterPlayer(this);
        }

        Canvas canvas = Object.FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        GameObject obj = Instantiate(healthBarPrefab, canvas.transform);
        bar = obj.GetComponent<HealthBar>();

        if (bar == null)
        {
            Debug.LogError("The HealthBar prefab does not contain the HealthBar component!");
            return;
        }

        bar.target = transform;
        bar.SetMaxValue(maxHealth);
    }

    public void TakeDamage(int amount, int explosionID)
    {
        if (isDead) return;

        PlayerSafeDome dome = GetComponent<PlayerSafeDome>();
        if (dome != null && dome.IsActive)
        {
            Debug.Log("🛡 Safe Dome active — damage ignored");
            return;
        }

        if (explosionsHit.Contains(explosionID)) return;
        explosionsHit.Add(explosionID);

        currentHealth -= amount;
        if (bar != null)
            bar.SetValue(currentHealth);

        Debug.Log($"{gameObject.name} took damage! Lives left: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }


    private void Die()
    {
        if (isDead) return;
        
        isDead = true;

        if (bar != null)
            Destroy(bar.gameObject);

        PlayerManager playerManager = PlayerManager.Instance;
        if (playerManager != null)
        {
            playerManager.OnPlayerDied(this);
        }

        Destroy(gameObject, 0.2f);
    }

    public int CurrentHealth => currentHealth;
}