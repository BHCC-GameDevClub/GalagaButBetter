using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    private int tempFPS;
    private int tempRes;
    private int tempLang;
    private bool tempFS;
    private bool tempVSync;
    private float tempMaster, tempMusic, tempSFX;
    private int tempColorBlind;
    private float tempHoldTime;

    [SerializeField] private TMP_Dropdown languageDropdown;

    [Header("Video Settings")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Toggle fps30;
    [SerializeField] private Toggle fps60;
    [SerializeField] private Toggle fps144;
    [SerializeField] private Toggle fpsUnlimited;

    [Header("Accessibility")]
    [SerializeField] private TMP_Dropdown colorBlindDropdown;
    [SerializeField] private Slider holdTimeSlider;

    [Header("Confirmation UI")]
    [SerializeField] private GameObject unsavedChangesPopup;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Controller UI")]
    [SerializeField] private GameObject controlsPanel;
    
    void Start()
    {
        if (GameManager.Instance == null) return;
        Debug.Log("MENU STARTING. GameManager says FPS is: " + GameManager.Instance.CurrentFPSLimit);

    // ++++++++++++++++ Temp Variables
        tempFPS = GameManager.Instance.CurrentFPSLimit;
        tempFS = PlayerPrefs.GetInt("SavedFS", 1) == 1;
        tempVSync = QualitySettings.vSyncCount > 0;
        tempMaster = PlayerPrefs.GetFloat("SavedMaster", 0.75f);
        tempMusic = PlayerPrefs.GetFloat("SavedMusic", 0.75f);
        tempSFX = PlayerPrefs.GetFloat("SavedSFX", 0.75f);
        tempColorBlind = PlayerPrefs.GetInt("SavedColorBlind", 0);


    // ++++++++++++++++ Language Setup
        if (languageDropdown != null)
        {
            tempLang = GameManager.Instance.CurrentLanguageIndex;
            languageDropdown.value = tempLang;
            languageDropdown.onValueChanged.RemoveAllListeners();
            languageDropdown.onValueChanged.AddListener(delegate { tempLang = languageDropdown.value; Debug.Log("Temp Language set to: " + tempLang); });
        }

            // Debug.LogError("FATAL: EventSystem.current is NULL!"); 

    // ++++++++++++++++ Resolution Setup
        if (resolutionDropdown != null)
        {
            GameManager.Instance.PopulateResolutions(resolutionDropdown);
            tempRes = PlayerPrefs.GetInt("SavedRes", resolutionDropdown.value); 
            resolutionDropdown.value = tempRes;
            resolutionDropdown.RefreshShownValue();

            resolutionDropdown.onValueChanged.RemoveAllListeners();
            resolutionDropdown.onValueChanged.AddListener(delegate { tempRes = resolutionDropdown.value;
            Debug.Log("Temp Resolution Index set to: " + tempRes); });
        }

    // ++++++++++++++++ Accessibility Setup
        if (colorBlindDropdown != null)
        {
            tempColorBlind = PlayerPrefs.GetInt("SavedColorBlind", 0);
            colorBlindDropdown.value = tempColorBlind;
            colorBlindDropdown.onValueChanged.RemoveAllListeners();
            colorBlindDropdown.onValueChanged.AddListener(delegate { tempColorBlind = colorBlindDropdown.value; Debug.Log("Temp ColorBlind set to:" + tempColorBlind); });
        }

        if (holdTimeSlider != null)
        {
            holdTimeSlider.onValueChanged.RemoveAllListeners();

            float savedValue = PlayerPrefs.GetFloat("AccessibilityHoldTime", 0.5f);

            tempHoldTime = savedValue;
            holdTimeSlider.value = savedValue;

            holdTimeSlider.onValueChanged.AddListener(delegate { tempHoldTime = holdTimeSlider.value; Debug.Log("Temp Hold Time set to:" + tempHoldTime); } );
        }

    // ++++++++++++++++ Fullscreen Setup
        if (fullscreenToggle != null)
        {
            tempFS = PlayerPrefs.GetInt("SavedFS", 1) == 1;

            fullscreenToggle.SetIsOnWithoutNotify(tempFS);
            fullscreenToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.onValueChanged.AddListener(delegate { tempFS = fullscreenToggle.isOn; });
        }

    // ++++++++++++++++ VSync Setup
        if (vsyncToggle != null)
        {
            vsyncToggle.isOn = tempVSync;
            vsyncToggle.onValueChanged.RemoveAllListeners();
            vsyncToggle.onValueChanged.AddListener(delegate {tempVSync = vsyncToggle.isOn; });
        }

    // ++++++++++++++++ FPS Setup
        SyncFPSToggles(tempFPS);
        fps30.onValueChanged.AddListener(delegate { if(fps30.isOn) tempFPS = 30; Debug.Log("Temp FPS to 30"); });
        fps60.onValueChanged.AddListener(delegate { if(fps60.isOn) tempFPS = 60; Debug.Log("Temp FPS to 60"); });
        fps144.onValueChanged.AddListener(delegate { if(fps144.isOn) tempFPS = 144; Debug.Log("Temp FPS to 144"); });
        fpsUnlimited.onValueChanged.AddListener(delegate { if(fpsUnlimited.isOn) tempFPS = -1; Debug.Log("Temp FPS to Unlimited");  });

    // ++++++++++++++++ Audio
        masterSlider.value = tempMaster;
        musicSlider.value = tempMusic;
        sfxSlider.value = tempSFX;

        ApplyVolumeToMixer("MasterVol", tempMaster);
        ApplyVolumeToMixer("MusicVol", tempMusic);
        ApplyVolumeToMixer("SFXVol", tempSFX);

        masterSlider.onValueChanged.AddListener(delegate {tempMaster = masterSlider.value; ApplyVolumeToMixer("MasterVol", tempMaster); });
        musicSlider.onValueChanged.AddListener(delegate { tempMusic = musicSlider.value; ApplyVolumeToMixer("MusicVol", tempMusic); });
        sfxSlider.onValueChanged.AddListener(delegate { tempSFX = sfxSlider.value; ApplyVolumeToMixer("SFXVol", tempSFX); }); 

        if (unsavedChangesPopup != null) unsavedChangesPopup.SetActive(false);   
    }

    private void SyncFPSToggles(int currentFPS)
    {
        fps30.SetIsOnWithoutNotify(currentFPS == 30);
        fps60.SetIsOnWithoutNotify(currentFPS == 60);
        fps144.SetIsOnWithoutNotify(currentFPS == 144);
        fpsUnlimited.SetIsOnWithoutNotify(currentFPS == -1);
        Debug.Log("UI Visuals synced to:" + currentFPS);        
    }

    private void ApplyVolumeToMixer(string parameteName, float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, sliderValue)) * 20;
        mainMixer.SetFloat(parameteName, dB);
    }

// ============================ Temp & GameManager difference checker
    private bool HasUnsavedChanges()
    {
        if (tempFPS != PlayerPrefs.GetInt("SavedFPSLimit", -1)) return true;
        if (tempRes != PlayerPrefs.GetInt("SavedRes", 0)) return true;
        if (tempColorBlind != PlayerPrefs.GetInt("SavedColorBlind", 0)) return true;
        if (tempLang != GameManager.Instance.CurrentLanguageIndex) return true;

        bool savedFS = PlayerPrefs.GetInt("SavedFS" , 1) == 1;
        if (tempFS != savedFS) return true;

        bool savedVSync = PlayerPrefs.GetInt("SavedVSync", 0) == 1;
        if (tempVSync != (QualitySettings.vSyncCount > 0)) return true;

        if (!Mathf.Approximately(tempMaster, PlayerPrefs.GetFloat("SavedMaster", 0.75f))) return true;
        if (!Mathf.Approximately(tempMusic, PlayerPrefs.GetFloat("SavedMusic", 0.75f))) return true;
        if (!Mathf.Approximately(tempSFX, PlayerPrefs.GetFloat("SavedSFX", 0.75f))) return true;
        if (!Mathf.Approximately(tempHoldTime, PlayerPrefs.GetFloat("AccessibilityHoldTime", 0.5f))) return true;

        return false;
    }


// ============================ OnApply
    public void OnApplyPressed()
    {
        Debug.Log($"Applying: FPS: {tempFPS}, Res Index: {tempRes}, Fullscreen: {tempFS}, VSync: {tempVSync}");

        GameManager.Instance.SetFramerate(tempFPS);
        GameManager.Instance.SetResolution(tempRes);
        GameManager.Instance.SetFullscreen(tempFS);
        GameManager.Instance.SetVSync(tempVSync);
        GameManager.Instance.SetLanguage(tempLang);

        PlayerPrefs.SetInt("SavedFPSLimit", tempFPS);
        PlayerPrefs.SetInt("SavedRes", tempRes);
        PlayerPrefs.SetInt("SavedFS", tempFS ? 1 : 0);
        PlayerPrefs.SetInt("SavedVSync", tempVSync ? 1 : 0);
        PlayerPrefs.SetInt("SavedLanguage", tempLang);
        PlayerPrefs.SetInt("SavedColorBlind", tempColorBlind);
        PlayerPrefs.SetFloat("SavedMaster", tempMaster);
        PlayerPrefs.SetFloat("SavedMusic", tempMusic);
        PlayerPrefs.SetFloat("SavedSFX", tempSFX);
        PlayerPrefs.SetFloat("AccessibilityHoldTime", tempHoldTime);
        PlayerPrefs.Save();

        Debug.Log("CRITICAL SAVE CHECK: Slider was" + holdTimeSlider.value + "and tempHoldTime is" + tempHoldTime);

        GameManager.Instance.CurrentFPSLimit = tempFPS;

        Debug.Log("Settings Applied and Saved!");
    }
    public void AttemptReturn() // Called when Return is clicked
    {
        if (GameManager.Instance == null)
        {
            SceneManager.LoadScene(0);
            return;
        }

        if (HasUnsavedChanges())
        {
            unsavedChangesPopup.SetActive(true);
        }
        else
        {
            GameManager.Instance.ReturnFromOptions();
        }
    } 

    public void ConfirmReturnWithoutSaving()
    {
        float oldMaster = PlayerPrefs.GetFloat("SavedMaster", 0.75f);
        float oldMusic = PlayerPrefs.GetFloat("SavedMusic", 0.75f);
        float oldSFX = PlayerPrefs.GetFloat("SavedSFX", 0.75f);

        ApplyVolumeToMixer("MasterVol", oldMaster);
        ApplyVolumeToMixer("MusicVol", oldMusic);
        ApplyVolumeToMixer("SFXVol", oldSFX);

        GameManager.Instance.ReturnFromOptions();
    }

    public void ClosePopup()
    {
        unsavedChangesPopup.SetActive(false);
    }

    public void ToggleControlsPanel(bool isActive)
    {
        controlsPanel.SetActive(isActive);
    }
}
