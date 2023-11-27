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
        
        private void Start()
        {
            StartCoroutine(Download());
        }

        private IEnumerator Download()
        {
            try
            {
                string res = Fetcher.Get("tasks/16",
                    "60c1d692c0b18734016efb12ef5ab9627bcf375c");
                LocalStorage.SetValue("quiz", res);
                Task task = JsonConvert.DeserializeObject<Task>(res);
                titleText.text = task.Title;
                descriptionText.text = task.Description;
                questionText.text = $"{task.Questions.Length} questions";

            } catch (Exception e)
            {
                Error err = JsonConvert.DeserializeObject<Error>(e.Message);
                error.ShowError(err != null? err.Detail: e.Message);
            }
            loading.SetActive(false);
            
            yield return null;
        }
    }
}