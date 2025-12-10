using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;

    public GameObject healthBarPrefab;
    private HealthBar bar;

    private void Start()
    {
        currentHealth = maxHealth;

        // Находим канвас, чтобы спавнить UI внутри него
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("На сцене нет Canvas! Создай Canvas для HealthBar.");
            return;
        }

        // Создаем HealthBar в Canvas (НЕ в мире!)
        GameObject obj = Instantiate(healthBarPrefab, canvas.transform);
        bar = obj.GetComponent<HealthBar>();

        if (bar == null)
        {
            Debug.LogError("Префаб HealthBar НЕ содержит компонент HealthBar!");
            return;
        }

        bar.target = transform;
        bar.SetMaxValue(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        bar.SetValue(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}