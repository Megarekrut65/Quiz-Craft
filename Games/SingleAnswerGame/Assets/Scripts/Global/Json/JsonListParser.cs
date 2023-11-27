using System.Collections.Generic;
using UnityEngine;

namespace Global.Json
{
    /// <summary>
    /// Reads from file localization data and puts it to dictionary.
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public static class JsonListParser<TValueType> {
        public static SortedDictionary<string, TValueType> Parse(string path)
        {
            path += ".json";
            string jsonData = AllFileReader.Read(path);

            JsonList<TValueType> list = JsonUtility.FromJson<JsonList<TValueType>>(jsonData);
            
            var map = new SortedDictionary<string, TValueType>();
            if (list.items != null) {
                foreach (var item in list.items) {
                    map.Add(item.key, item.value);
                }
            }

            return map;
        }
    }
}