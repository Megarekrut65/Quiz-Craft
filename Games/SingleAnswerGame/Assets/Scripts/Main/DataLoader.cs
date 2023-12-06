using System;
using System.Collections;
using Global;
using Global.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class DataLoader : MonoBehaviour
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text questionText;

        [SerializeField] private GameObject loading;
        [SerializeField] private ErrorLogger error;

        private string _uid;
        private string _token;
        
        private void Start()
        {
            _token = "60c1d692c0b18734016efb12ef5ab9627bcf375c";
            _uid = "0agjrJWFNj4WKRSMstkiTi81gfr9RFMr";
            
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
                string res = Fetcher.Get("task-responses/"+_uid, _token);
                
                GameResponse response = JsonConvert.DeserializeObject<GameResponse>(res);
                Debug.Log(res);

            } catch (Exception e)
            {
                Debug.Log(e);
                Error err = JsonConvert.DeserializeObject<Error>(e.Message);
                error.ShowError(err != null? err.Detail: e.Message);
            }
        }
        private void DownloadGameData()
        {
            try
            {
                string res = Fetcher.Get("games/uid/get-game/"+_uid, _token);
                
                LocalStorage.SetValue("quiz", res);
                Game game = JsonConvert.DeserializeObject<Game>(res);
                titleText.text = game.Task.Title;
                descriptionText.text = game.Task.Description;
                questionText.text = $"{game.Task.Questions.Length} questions";

            } catch (Exception e)
            {
                Debug.Log(e);
                Error err = JsonConvert.DeserializeObject<Error>(e.Message);
                error.ShowError(err != null? err.Detail: e.Message);
            }
            
        }
    }
}