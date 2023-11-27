using System.Collections.Generic;
using Global.Json;

namespace Global.Localization
{
    public class LocalizationManagerInstance
    {
        private readonly string _languagePath;
        
        private SortedDictionary<string, string> _wordMap = new SortedDictionary<string, string>();
        private string _currentLanguage = "";

        public LocalizationManagerInstance(string languagePath, string language="")
        {
            _languagePath = languagePath;
            ChangeLanguage(language);
        }
        
        public void ChangeLanguage(string language) {
            if (language == _currentLanguage) {
                return;
            }

            _currentLanguage = language;
            _wordMap = JsonListParser<string>.Parse($"{_languagePath}/{language}");
        }
        
        public string GetWord(string key)
        {
            return _wordMap.TryGetValue(key, out var value) ? value : key;
        }
    }
}