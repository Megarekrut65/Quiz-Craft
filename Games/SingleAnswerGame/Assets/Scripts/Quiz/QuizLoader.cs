
using System.Collections;
using Global;
using Global.Data;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PlayerController;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz
{
    public class QuizLoader : MonoBehaviour
    {
        [SerializeField] private GameObject variantPrefab;

        [SerializeField] private GameObject contentObj;
        [SerializeField] private Transform content;
        
        [SerializeField] private Text text;

        [SerializeField] private EntityController player;
        [SerializeField] private EntityController enemy;

        private TaskResponse _response;
        private Task _task;

        private void Start()
        {
            string json = LocalStorage.GetValue("quiz", "");
            _task = JsonConvert.DeserializeObject<Task>(json);
            _response = new TaskResponse { Responses = new QuestionResponse[_task.Questions.Length] };
            
            if(_task.Questions.Length > 0) LoadQuestion(0, _task.Questions[0]);
        }
        
        private void LoadQuestion(int index, Question question)
        {
            if(question == null) return;
            contentObj.SetActive(true);

            text.text = question.Description;
            LoadAnswers(index, question.Answers);
        }
        
        private void LoadAnswers(int questionIndex, [CanBeNull] Answer[] answers)
        {
            foreach (Transform child in content) {
                Destroy(child.gameObject);
            }
            if(answers == null) return;

            foreach (Answer answer in answers)
            {
                GameObject obj = Instantiate(variantPrefab, content, false);
                
                obj.GetComponent<Variant>().SetData(questionIndex, answer, AnswerClick);
            }
        }

        private void AnswerClick(int questionIndex, Answer answer)
        {
            contentObj.SetActive(false);
            
            Question question = _task.Questions[questionIndex];
            _response.Responses[questionIndex] = new QuestionResponse { Question = question.Id, Answer = answer.Id };

            if (answer.Correct) player.Attack();
            else enemy.Attack();
            
            StartCoroutine(LoadNext(questionIndex + 1));
        }

        private IEnumerator LoadNext(int next)
        {
            yield return new WaitForSeconds(5f);
            
            if (next < _task.Questions.Length)
            {
                LoadQuestion(next, _task.Questions[next]);
            }
        }
    }
}