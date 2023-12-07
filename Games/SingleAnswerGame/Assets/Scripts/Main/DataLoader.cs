using System;
using System.Collections;
using System.Runtime.InteropServices;
using Global;
using Global.Data;
using Global.Localization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Main
{
    public class DataLoader : MonoBehaviour
    {
        [DllImport("__Internal")]
        [CanBeNull]
        private static extern string Token();

        [DllImport("__Internal")]
        [CanBeNull]
        private static extern string UidFromUrl();
        
        
        
        [SerializeField] private Text titleText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text questionText;

        [SerializeField] private GameObject loading;
        [SerializeField] private ErrorLogger error;
        private string _token;

        private string _uid;

        private void Start()
        {
            if (Application.isEditor)
            {
                _token = "60c1d692c0b18734016efb12ef5ab9627bcf375c";
                _uid = "0agjrJWFNj4WKRSMstkiTi81gfr9RFMr"; 
            }
            else{
                _token = "60c1d692c0b18734016efb12ef5ab9627bcf375c";//Token()??""; //
                _uid = "fkwAq11PMOIviQALUNwTNcTZzCDJq4Nd";//UidFromUrl()??""; //
            }

            Debug.Log($"Token: {_token}");
            Debug.Log($"Uid: {_uid}");

            LocalStorage.SetValue("token", _token);
            LocalStorage.SetValue("uid", _uid);

            try
            {
                Fetcher.Get(this, "games/uid/get-game/" + _uid, _token, LoadGame);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                try
                {
                    Error err = JsonConvert.DeserializeObject<Error>(e.Message);
                    error.ShowError(err != null ? err.Detail : e.Message);
                }
                catch (Exception e2)
                {
                    error.ShowError(e.Message);
                }
            }
        }

        private void LoadGame(string json)
        {
            LocalStorage.SetValue("quiz", json);
            Game game = JsonConvert.DeserializeObject<Game>(json);
            titleText.text = game.Task.Title;
            descriptionText.text = game.Task.Description;
            questionText.text = $"{game.Task.Questions.Length} {LocalizationManager.GetWordByKey("questions")}";
            
            loading.SetActive(false);
        }
    }
}