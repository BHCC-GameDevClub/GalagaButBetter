using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class HealthDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Drag Health Bar Images here:")]
    public List<Image> healthBarImages;

    [Header("Health Sprites")]
    [Tooltip("Drag activated health image here")]
    public Sprite fullHealthSprite;
    [Tooltip("Drag depleted health image here")]
    public Sprite damagedHealthSprite;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthVisuals;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthVisuals;
    }

    // HP Event
    private void UpdateHealthVisuals(int currentHealth, int maxHealth)
    {
        // Ensure list is populated
        if (healthBarImages == null || healthBarImages.Count == 0)
        {
            Debug.LogError("Health Bar Image List is EMPTY");
            return;
        }

    // maxHealth is equal to number of bars but we iterate up to number of bars we have 

     for (int i = 0; i < healthBarImages.Count; i++)
        {
            // if current index (+1) is less than or equial to current hp
            // FULL BAR
            if (i < currentHealth)
            {
                // Full HP
                healthBarImages[i].sprite = fullHealthSprite;
                healthBarImages[i].enabled = true; // Bar is visible
            }
            else
            {
                // Empty HP
                healthBarImages[i].sprite = damagedHealthSprite;
                healthBarImages[i].enabled = true; // Still visible but greyed out
            }
        }  
    }

}