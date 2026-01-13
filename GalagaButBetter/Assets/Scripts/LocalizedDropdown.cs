using UnityEngine;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class LocalizedDropdown : MonoBehaviour
{
    [Tooltip("List of keys matching the dropdown options order")]
    public List<string> localizationKeys;

    private TMP_Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        UpdateDropdown(GameManager.Instance.CurrentLanguageIndex);
        GameManager.OnLanguageChanged += UpdateDropdown;
    }

    void OnDestroy()
    {
        GameManager.OnLanguageChanged -= UpdateDropdown;
    }

    private void UpdateDropdown(int langIndex)
    {
        if (LocalizationManager.Instance == null) return;

        // Loop and Update
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if (i < localizationKeys.Count)
            {
                dropdown.options[i].text = LocalizationManager.Instance.GetLocalizedValue(localizationKeys[i]);
            }
        }
        // refresh
        dropdown.RefreshShownValue();
    }
}
