using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]

public class LocalizedText : MonoBehaviour
{
    [Tooltip("The key to look up in LocalizationManager (eg start_game)")]
    public string localizationKey;

    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        UpdateText(GameManager.Instance.CurrentLanguageIndex); // immediate update on start
        GameManager.OnLanguageChanged += UpdateText; // sub to event for changes
    }

    void OnDestroy() // Always Unsub from statics to prevent errors
    {
        GameManager.OnLanguageChanged -= UpdateText;
    }

    private void UpdateText(int languageIndex)
    {
        if (LocalizationManager.Instance != null)
        {
            textComponent.text = LocalizationManager.Instance.GetLocalizedValue(localizationKey);
        }
    }
}
