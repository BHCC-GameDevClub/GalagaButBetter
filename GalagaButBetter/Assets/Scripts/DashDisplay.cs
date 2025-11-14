using UnityEngine;
using UnityEngine.UI;

public class DashDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image dashIconImage;

    [Header("Dash Sprites")]
    [SerializeField] private Sprite dashReadySprite;
    [SerializeField] private Sprite dashCooldownSprite;

    private void OnEnable()
    {
        PlayerController.OnDashStateChanged += UpdateIcon;
    }

    private void OnDisable()
    {
        PlayerController.OnDashStateChanged -= UpdateIcon;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dashIconImage.sprite = dashReadySprite;

    }
    
    private void UpdateIcon(bool isReady)
    {
        if (isReady)
        {
            dashIconImage.sprite = dashReadySprite;
        }
        else
        {
            dashIconImage.sprite = dashCooldownSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
