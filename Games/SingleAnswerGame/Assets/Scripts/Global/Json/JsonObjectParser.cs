using UnityEngine;

namespace Global.Json
{
    public static class JsonObjectParser<TValueType>
    {
        public static TValueType Parse(string path)
        {
            path += ".json";
            string jsonData = AllFileReader.Read(path);

            return JsonUtility.FromJson<TValueType>(jsonData);
        }
    }
}