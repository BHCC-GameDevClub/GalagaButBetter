using UnityEngine;

public class Enemy_Health : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth;


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
            Destroy(gameObject);
        }
    }



}
