using System.Collections;
using System.Collections.Generic;
using Global.Json;
using UnityEngine;

namespace Global.Localization
{
    /// <summary>
    /// Loads data from file and gets translated words by keys.
    /// </summary>
    public class LocalizationManager : MonoBehaviour {
        private const string LanguageKey = "Language";
        private const string LanguageFolder = "Languages/";

        public static LocalizationManager Instance { get; private set; }
        public bool Ready { get; private set; } = true;

        private readonly LocalizationManagerInstance _instance = new LocalizationManagerInstance(
            $"{Application.streamingAssetsPath}/{LanguageFolder}");

        public delegate void ChangeLanguageText();
        public event ChangeLanguageText OnLanguageChanged;

        public void ChangeLanguage(string language) {
            PlayerPrefs.SetString(LanguageKey, language);
            Ready = false;
            StartCoroutine(ChangeCoroutine(language));
        }

        private IEnumerator ChangeCoroutine(string language)
        {
            _instance.ChangeLanguage(language);
            yield return null;
            Ready = true;
            OnLanguageChanged?.Invoke();
        }
        private void SetLanguagePrefab()
        {
            if (PlayerPrefs.HasKey(LanguageKey)) return;
            switch (Application.systemLanguage) {
                case SystemLanguage.Ukrainian:
                    PlayerPrefs.SetString(LanguageKey, "uk_UK");
                    break;
                default:
                    PlayerPrefs.SetString(LanguageKey, "en_US");
                    break;
            }
        }

        private void Awake() {
            SetLanguagePrefab();
            if (Instance == null) {
                Instance = this;
                ChangeLanguage(PlayerPrefs.GetString(LanguageKey));
            } else if (Instance != this) {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
        public static string GetWordByKey(string key)
        {
            return Instance == null ? key : Instance._instance.GetWord(key);
        }
    }
}