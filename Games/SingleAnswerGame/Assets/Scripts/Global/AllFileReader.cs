using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Global
{
    public static class AllFileReader {

        private static bool IsPhone()
        {
            return Application.platform == RuntimePlatform.Android
                   || Application.platform == RuntimePlatform.IPhonePlayer;
        }
        public static string Read(string path) {
            string res;
            if (IsPhone()) {
                UnityWebRequest www = UnityWebRequest.Get(path);
                www.SendWebRequest();
                while (!www.isDone) {
                }
                
                res = www.downloadHandler.text;
            } else {
                res = File.ReadAllText(path);
            }

            return res;
        }
        public static byte[] ReadBytes(string path) {
            byte[] res;
            
            if (IsPhone()) {
                UnityWebRequest www = UnityWebRequest.Get(path);
                www.SendWebRequest();
                while (!www.isDone) {
                }

                res = www.downloadHandler.data;
            } else
            {
                res = File.ReadAllBytes(path);
            }

            return res;
        }
    }
}