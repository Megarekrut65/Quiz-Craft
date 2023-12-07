using System.Collections.Generic;
using Global.Json;
using JetBrains.Annotations;
using UnityEngine;

namespace Global.Localization
{
    public class LocalizationManagerInstance
    {
        [CanBeNull] private Language _current;

        private SortedDictionary<string, string> _wordMap = new SortedDictionary<string, string>();

        public void ChangeLanguage(Language language)
        {
            if (_current != null && language.code == _current.code) return;

            _current = language;
            _wordMap = JsonListParser<string>.ParseContent(language.asset.text);
        }

        public string GetWord(string key)
        {
            return _wordMap.TryGetValue(key, out var value) ? value : key;
        }
    }
}