using System;
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
        [CanBeNull]
        private static extern string Username();
        
        [SerializeField] private Text titleText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text questionText;
        [SerializeField] private Text alreadyText;

        [SerializeField] private GameObject loading;
        [SerializeField] private ErrorLogger logger;
        
        private string _token;
        private string _uid;


        [CanBeNull] private GameResponse _response;
        [CanBeNull] private Game _game;

        private void Start()
        {
            string username;
            
            if (Application.isEditor)
            {
                _token = "422e91d51775a99e39b979b1fdb6c7904c50bab3";
                _uid = "RPvP1BVDe0G1YhWabXCy2UZuXKoYAQXx";
                username = "Editor";
            }
            else{
                _token = Token()??"";
                _uid = UidFromUrl()??"";
                username = Username()??"";
            }

            if (string.IsNullOrEmpty(_token))
            {
                logger.ShowError(LocalizationManager.GetWordByKey("auth"));
                return;
            }
            if (string.IsNullOrEmpty(_uid))
            {
                logger.ShowError(LocalizationManager.GetWordByKey("uid"));
                return;
            }

            LocalStorage.SetValue("token", _token);
            LocalStorage.SetValue("uid", _uid);
            LocalStorage.SetValue("username", username);

            Fetcher.Get(this, "games/uid/get-game/" + _uid, _token, LoadGame);
        }

        private void LoadGame([CanBeNull] Error error, string json)
        {
            if (error != null)
            {
                logger.ShowError(error.Detail);
                return;
            }
            

            _game = JsonConvert.DeserializeObject<Game>(json);
            if (_game == null)
            {
                logger.ShowError(LocalizationManager.GetWordByKey("error"));
                return;
            }
            
            LocalStorage.SetValue("quiz", json);

            LoadGameText();
            
            Fetcher.Get(this, "game-responses/" + _uid, _token, LoadResponse);
        }
        private void LoadResponse([CanBeNull] Error error, string json)
        {
            LocalStorage.SetValue("response", "");
            if (error != null)
            {
                if (error.Detail == "No response found for the given game and user, or owner closed the game.")
                {
                    loading.SetActive(false);
                    return;
                }
                
                logger.ShowError(error.Detail);
                return;
            }
            
            LocalStorage.SetValue("response", json);

            _response = JsonConvert.DeserializeObject<GameResponse>(json);
            if (_response?.Responses == null)
            {
                loading.SetActive(false);
                return;
            }

            LoadResponseText();
            
            loading.SetActive(false);
        }

        private void LoadGameText()
        {
            if(_game == null) return;
            
            titleText.text = _game.Task.Title;
            descriptionText.text = _game.Task.Description;
            questionText.text = $"{_game.Task.Questions.Length} {LocalizationManager.GetWordByKey("questions")}";
        }

        private void LoadResponseText()
        {
            if (_response?.Responses != null && _response.Responses.Length != 0)
            {
                alreadyText.text = LocalizationManager.GetWordByKey("already");
            }
        }

        private void Awake()
        {
            LocalizationManager.Instance.OnLanguageChanged += LoadGameText;
            LocalizationManager.Instance.OnLanguageChanged += LoadResponseText;
        }

        private void OnDestroy()
        {
            LocalizationManager.Instance.OnLanguageChanged -= LoadGameText;
            LocalizationManager.Instance.OnLanguageChanged -= LoadResponseText; 
        }
    }
}