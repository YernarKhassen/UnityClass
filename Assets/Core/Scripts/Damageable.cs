using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
    }
}