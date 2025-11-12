using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} получил урон! Осталось жизней: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} погиб!");
        Destroy(gameObject);
    }
}