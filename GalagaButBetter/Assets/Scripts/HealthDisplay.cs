using UnityEngine.UI;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("The Image component that will change.")]
    public Image healthImage;

    [Header("Health Sprites")]
    [Tooltip("Sprite for 4 HP")]
    public Sprite fullHealthSprite;
    [Tooltip("Sprite for 2 HP(1 Dmg)")]
    public Sprite oneDamageSprite;
    [Tooltip("SPrite for 1 HP (2 Dmg)")]
    public Sprite twoDamageSprite;
    [Tooltip("Sprite for 0 HP")]
    public Sprite emptyHealthSprite;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealth;
    }

    // HP Event
    private void UpdateHealth(int currentHealth, int maxHealth)
    {
     switch (currentHealth)
           {
            case 3:
                healthImage.sprite = fullHealthSprite;
                break;
            case 2:
                healthImage.sprite = oneDamageSprite;
                break;
            case 1:
                healthImage.sprite = twoDamageSprite;
                break;
            case 0:
                healthImage.sprite = emptyHealthSprite;
                break;
            default:
                if (currentHealth > 3)
                {
                    healthImage.sprite = fullHealthSprite;
                }
                else
                {
                    healthImage.sprite = emptyHealthSprite;
                }
                break;
        }   
    }

}