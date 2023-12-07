using System.Collections;
using UnityEngine;

namespace Global.Localization
{
    /// <summary>
    /// Loads data from file and gets translated words by keys.
    /// </summary>
    public class LocalizationManager : MonoBehaviour
    {
        [SerializeField] private Language[] languages;

        private const string LanguageKey = "language";
        public delegate void ChangeLanguageText();

        private LocalizationManagerInstance _instance = new LocalizationManagerInstance();

        public static LocalizationManager Instance { get; private set; }
        public bool Ready { get; private set; } = true;

        private void Awake()
        {
            SetLanguagePrefab();
            if (Instance == null)
            {
                Instance = this;
                ChangeLanguage(PlayerPrefs.GetString(LanguageKey, "en_US"));
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public event ChangeLanguageText OnLanguageChanged;

        public void ChangeLanguage(string languageCode)
        {
            foreach (Language language in languages)
            {
                if(language.code != languageCode) continue;
                
                PlayerPrefs.SetString(LanguageKey, languageCode);
                Ready = false;
                StartCoroutine(ChangeCoroutine(language));
            }
            
        }

        private IEnumerator ChangeCoroutine(Language language)
        {
            _instance.ChangeLanguage(language);
            yield return null;
            Ready = true;
            OnLanguageChanged?.Invoke();
        }

        private void SetLanguagePrefab()
        {
            if (PlayerPrefs.HasKey(LanguageKey)) return;
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Ukrainian:
                    PlayerPrefs.SetString(LanguageKey, "uk_UK");
                    break;
                default:
                    PlayerPrefs.SetString(LanguageKey, "en_US");
                    break;
            }
        }

        public static string GetWordByKey(string key)
        {
            return Instance == null ? key : Instance._instance.GetWord(key);
        }
    }
}