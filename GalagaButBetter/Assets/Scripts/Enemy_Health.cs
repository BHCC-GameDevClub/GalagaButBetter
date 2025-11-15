using UnityEngine;

public class Enemy_Health : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth;

    [Tooltip("How many points this enemy is worth")]
    public int pointsOnDeath = 100;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. Current health: " + currentHealth); // dmg log

        if (currentHealth <= 0)
        {
            GameManager.Instance.AddScore(pointsOnDeath);
            
            Destroy(gameObject);
        }
    }
}



