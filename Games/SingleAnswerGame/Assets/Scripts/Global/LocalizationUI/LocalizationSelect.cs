using System.Collections;
using Global.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Global.LocalizationUI
{
    public class LocalizationSelect : MonoBehaviour
    {
        [SerializeField] private LanguageLoader languageLoader;

        [SerializeField] private LocalizationItem[] items;

        [SerializeField] private Image icon;

        private int _selected;

        private void Awake()
        {
            LocalizationManager.Instance.OnLanguageChanged += ChangeLanguage;
        }

        private void Start()
        {
            if (items.Length == 0) return;

            _selected = LocalStorage.GetValue("language", 0);

            icon.sprite = items[_selected].icon;
            languageLoader.SetActive(true);
            LocalizationManager.Instance.ChangeLanguage(items[_selected].languageCode);
        }

        private void OnDestroy()
        {
            LocalizationManager.Instance.OnLanguageChanged -= ChangeLanguage;
        }

        public void SelectNext()
        {
            if (_selected + 1 < items.Length)
            {
                _selected++;
            }
            else
            {
                _selected = 0;
            }

            LocalStorage.SetValue("language", _selected);
            icon.sprite = items[_selected].icon;

            languageLoader.SetActive(true);
            LocalizationManager.Instance.ChangeLanguage(items[_selected].languageCode);
        }

        private void ChangeLanguage()
        {
            StartCoroutine(Hide());
        }

        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(0.5f);
            languageLoader.SetActive(false);
        }
    }
}