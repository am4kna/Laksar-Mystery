using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Reference")]
    public healthbar healthBar; // Reference to your health bar script

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setmaxhealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Keep health between 0 and max

        healthBar.sethealth(currentHealth);

        Debug.Log("Player took " + damage + " damage. Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.sethealth(currentHealth);
    }

    void Die()
    {
        Debug.Log("Player died!");
        // Add death logic here (restart level, game over screen, etc.)
    }
}
