using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth;
    public static event Action<int, int> OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("G key pressed. dleaing 1 test damage.");
            ChangeHealth(1);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);    
        }
    }
}
