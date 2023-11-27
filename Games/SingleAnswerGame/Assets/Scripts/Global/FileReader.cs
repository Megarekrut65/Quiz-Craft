using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Global
{
    public static class FileReader
    {
        public static Sprite ReadSprite(string fullPath, int width=2, int height=2)
        {
            byte[] bytes = AllFileReader.ReadBytes(fullPath);
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f), 100.0f);
 
            return sprite;
        }

        public static AudioClip ReadAudio(string name, string fullPath)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullPath,
                AudioType.MPEG);
            www.SendWebRequest();
            while (!www.isDone) {
            }
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            clip.name = name;

            return clip;
        }
    }
}