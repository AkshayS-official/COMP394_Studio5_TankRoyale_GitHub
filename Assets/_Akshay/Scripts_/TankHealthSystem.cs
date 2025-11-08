using UnityEngine;

public class TankHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 3f;
    [Tooltip("The current health of the tank.")]
    public float CurrentHealth;

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    // Public method to take damage
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Max(0, CurrentHealth); // Ensure health doesn't go below zero

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // Handle tank destruction
    private void Die()
    {
        // You would typically instantiate an explosion particle system here
        Debug.Log(gameObject.name + " has been destroyed!");

        // Deactivate or destroy the tank model
        gameObject.SetActive(false);
        // Or: Destroy(gameObject);
    }
}