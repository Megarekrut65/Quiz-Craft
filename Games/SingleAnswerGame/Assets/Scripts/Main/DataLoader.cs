using System;
using System.Collections;
using System.Runtime.InteropServices;
using Global;
using Global.Data;
using Global.Localization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
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
        
        [DllImport("__Internal")]

        private static extern string StringReturnValueFunction();
        
        
        [SerializeField] private Text titleText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text questionText;

        [SerializeField] private GameObject loading;
        [SerializeField] private ErrorLogger error;
        private string _token;

        private string _uid;

        private void Start()
        {
            Debug.Log($"return: {StringReturnValueFunction()}");
            if (Application.isEditor)
            {
                _token = "60c1d692c0b18734016efb12ef5ab9627bcf375c";
                _uid = "0agjrJWFNj4WKRSMstkiTi81gfr9RFMr"; 
            }
            else{
                _token = Token()??""; //"60c1d692c0b18734016efb12ef5ab9627bcf375c";
                _uid = UidFromUrl()??"unknown"; //"0agjrJWFNj4WKRSMstkiTi81gfr9RFMr";
            }

            Debug.Log($"Token: {_token}");
            Debug.Log($"Uid: {_uid}");
            return;
            LocalStorage.SetValue("token", _token);
            LocalStorage.SetValue("uid", _uid);

            StartCoroutine(Download());
        }

        private IEnumerator Download()
        {
            //DownloadResponseData();
            DownloadGameData();
            loading.SetActive(false);
            yield return null;
        }

        private void DownloadResponseData()
        {
            try
            {
                string res = Fetcher.Get("task-responses/" + _uid, _token);

                GameResponse response = JsonConvert.DeserializeObject<GameResponse>(res);
                Debug.Log(res);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Error err = JsonConvert.DeserializeObject<Error>(e.Message);
                error.ShowError(err != null ? err.Detail : e.Message);
            }
        }

        private void DownloadGameData()
        {
            try
            {
                string res = Fetcher.Get("games/uid/get-game/" + _uid, _token);

                LocalStorage.SetValue("quiz", res);
                Game game = JsonConvert.DeserializeObject<Game>(res);
                titleText.text = game.Task.Title;
                descriptionText.text = game.Task.Description;
                questionText.text = $"{game.Task.Questions.Length} {LocalizationManager.GetWordByKey("questions")}";
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
    }
}