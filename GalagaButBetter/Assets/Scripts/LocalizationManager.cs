using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private Dictionary<string, string[]> localizedText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLocalizedText();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLocalizedText()
    {
        localizedText = new Dictionary<string, string[]>();

        //  Format: Key> [English, Russian, Spanish]

        // ++++++++++++++++++++++++++ Keys

        // Menus
        localizedText.Add("paused", new string[] { "Paused", "Пауза", "Pausa" });
        localizedText.Add("resume", new string[] { "Resume", "Продолжить", "Continuar" });
        localizedText.Add("main_menu", new string[] { "Main Menu", "Главное меню", "Menu" });
        localizedText.Add("respawn", new string[] { "Respawn", "Возрождение", "Spawnear" });
        localizedText.Add("return", new string[] { "Return", "возвращаться", "Regresar" });
        localizedText.Add("apply", new string[] { "Apply", "применять", "Aplicar" });

        // Options
        localizedText.Add("general", new string[] { "General", "Общие", "General" });
        localizedText.Add("audio", new string[] { "Audio", "Звук", "Audio" });
        localizedText.Add("controls", new string[] { "Controls", "Управление", "Controles" });
        localizedText.Add("accessibility", new string[] { "Accessibility", "Доступность", "Accesibilidad" });

        // Generic Labels
        localizedText.Add("language_label", new string[] { "Language:", "Язык:", "Idioma:" });
        localizedText.Add("credits_label", new string[] { "Credits:", "Титры:", "Creditos:" });
        localizedText.Add("shoutouts_label", new string[] { "Shoutouts:", "Переклички:", "Menciones:" });

        // Video Settings
        localizedText.Add("graphics_section", new string[] { "Graphics", "Графика", "Graficos" });
        localizedText.Add("resolution_label", new string[] { "Resolution:", "Разрешение:", "Resolucion:" });
        localizedText.Add("fullscreen_label", new string[] { "Fullscreen:", "Полноэкранный режим:", "Modo Pantalla" });
        localizedText.Add("unlimited", new string[] { "Unlimited", "Неограниченная", "Ilimitado" });

        // Audio Settings
        localizedText.Add("master_vol", new string[] { "Master:", "Общая громкость:", "General:" });
        localizedText.Add("music_vol", new string[] { "Music:", "Музыка:", "Musica:" });
        localizedText.Add("sfx_vol", new string[] { "SFX:", "Эффекты:", "Efectos:" });

        // Accessibility Settings
        localizedText.Add("colorswap_label", new string[] { "Color Swap:", "Полноцветный фильтр:", "Daltonismo:" });
        localizedText.Add("holdtime_label", new string[] { "Hold Time:", "Время задержки ввода", "Tiempo de Presion:" });

        // Unsaved Changes Pop-up
        localizedText.Add("unsaved_popup", new string[] {
            "\nYou have unsaved changes.\nAre you sure you want to continue?",
            "\nЕсть несохраненные изменения.\nВы уверены, что хотите продолжить?",
            "\nTienes cambios sin guardar.\nEstas seguro de continuar?"
        });

        localizedText.Add("yes", new string[] { "Yes", "да", "Si" });
        localizedText.Add("no", new string[] { "No", "нет", "No" });

        // Control Bindings
        localizedText.Add("movement", new string[] { "Movement", "движение", "Movimiento" });
        localizedText.Add("dash", new string[] { "Dash", "Рывок", "Dash" });
        localizedText.Add("teleport", new string[] { "Teleport", "Телепорт", "Teletransporte" });
        localizedText.Add("fire", new string[] { "Fire", "Огонь", "Disparar" });

        // Other Text
        localizedText.Add("start_game", new string[] { "Start", "Начать", "Iniciar" });
        localizedText.Add("options", new string[] { "Options", "Настройки", "Opciones" });
        localizedText.Add("quit", new string[] { "Quit", "Выйти", "Salir" });
        localizedText.Add("Return", new string[] { "Return", "Вернуться", "Regresar" });
        localizedText.Add("score_label", new string[] { "Score: ", "Очки: ", "Puntaje: " });

        // Dropdowns
        localizedText.Add("cb_off", new string[] { "Off", "выключенный", "Apagado" });
        localizedText.Add("cb_protanopia", new string[] { "Protanopia", "Протанопия", "Protanopia" });
        localizedText.Add("cb_deuteranopia", new string[] { "Deuteranopia", "Дейтеранопия", "Deuteranopia" });

    }

    public string GetLocalizedValue(string key)
    {
        if (!localizedText.ContainsKey(key))
        {
            return "KEY NOT FOUND";
        }

        string[] results = localizedText[key];
        int langIndex = GameManager.Instance.CurrentLanguageIndex;

        if (langIndex >= results.Length)
        {
            return results[0]; // English Default
        }

        return results[langIndex];
    }

}
