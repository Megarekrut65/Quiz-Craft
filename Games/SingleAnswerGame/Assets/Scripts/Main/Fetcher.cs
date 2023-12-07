using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Main
{
    public static class Fetcher
    {
        private const string ServerUrl = "http://127.0.0.1:8000/";

        private static string SendAsync(UnityWebRequest request)
        {
            request.SendWebRequest();
            while (!request.isDone)
            {
            }

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                throw new Exception(request.downloadHandler.text);
            }

            return request.downloadHandler.text;
        }

        public static string Get(string endpoint, string token)
        {
            UnityWebRequest request = new UnityWebRequest();
            request.downloadHandler = new DownloadHandlerBuffer();
            request.url = $"{ServerUrl}api/{endpoint}/";
            request.SetRequestHeader("Content-Type" , "application/json; charset=UTF-8");
            request.SetRequestHeader("Authorization", $"Token {token}");
            request.method = UnityWebRequest.kHttpVerbGET;
            

            return SendAsync(request);
        }

        public static string Post(string endpoint, string token, WWWForm form)
        {
            UnityWebRequest request = UnityWebRequest.Post($"{ServerUrl}api/{endpoint}/", form);
            request.SetRequestHeader("Authorization", $"Token {token}");

            return SendAsync(request);
        }
    }
}