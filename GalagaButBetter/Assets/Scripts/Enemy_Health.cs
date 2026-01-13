using UnityEngine;
using System;

public class Enemy_Health : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth;

    [Tooltip("How many points this enemy is worth")]
    public int pointsOnDeath = 100;
    public event Action<int, int> OnHealthChanged;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log(gameObject.name + " took " + amount + " damage. Current health: " + currentHealth); // dmg log

        if (currentHealth <= 0)
        {
            GameManager.Instance.AddScore(pointsOnDeath);

            Destroy(gameObject);
        }
    }
}



