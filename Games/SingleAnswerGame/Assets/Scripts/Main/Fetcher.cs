using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Main
{
    public static class Fetcher
    {
        private const string ServerUrl = "http://127.0.0.1:8000/";
        
        private static IEnumerator GetNumerator(string endpoint, string token, Action<string> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Get($"{ServerUrl}api/{endpoint}/"))
            {
                request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
                request.SetRequestHeader("Authorization", $"Token {token}");

                yield return request.SendWebRequest();
                while (!request.isDone)
                {
                    yield return null;
                }

                if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
                {
                    throw new Exception(request.downloadHandler.text);
                }
                
                string res = request.downloadHandler.text;
                callback(res);
            }
        }
        
        public static void Get(MonoBehaviour behaviour, string endpoint, string token, Action<string> callback)
        {
            behaviour.StartCoroutine(GetNumerator(endpoint, token, callback));
        }
        
        private static IEnumerator PostNumerator(string endpoint, string token, WWWForm form, Action<string> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Post($"{ServerUrl}api/{endpoint}/", form))
            {
                request.SetRequestHeader("Authorization", $"Token {token}");

                yield return request.SendWebRequest();
                while (!request.isDone)
                {
                    yield return null;
                }

                if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
                {
                    throw new Exception(request.downloadHandler.text);
                }
                
                string res = request.downloadHandler.text;
                callback(res);
            }
        }
        
        public static void Post(MonoBehaviour behaviour, string endpoint, string token, WWWForm form, Action<string> callback)
        {
            behaviour.StartCoroutine(PostNumerator(endpoint, token, form, callback));
        }
    }
}