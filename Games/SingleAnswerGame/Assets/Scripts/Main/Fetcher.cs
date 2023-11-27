using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Main
{
    public static class Fetcher
    {
        private const string ServerUrl = "http://127.0.0.1:8000/";
        
        private static string SendAsync(UnityWebRequest request)
        {
            request.SendWebRequest();
            while (!request.isDone) {
            }

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                throw new Exception(request.downloadHandler.text);
            }
            
            return request.downloadHandler.text;
        }

        public static string Get(string endpoint, string token)
        {
            UnityWebRequest request = UnityWebRequest.Get($"{ServerUrl}api/{endpoint}/");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Token {token}");

            return SendAsync(request);
        }
    }
}