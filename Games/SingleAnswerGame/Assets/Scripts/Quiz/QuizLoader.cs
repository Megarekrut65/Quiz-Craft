
using System;
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
        
        [SerializeField] private Text questionText;
        [SerializeField] private Text gradeText;

        [SerializeField] private EntityController player;
        [SerializeField] private EntityController enemy;

        [SerializeField] private GameOverController gameOverController;

        private TaskResponse _response;
        private Task _task;

        private int _grade = 0;
        private int _maxGrade = 0;
        
        private int _correct = 0;

        private void Start()
        {
            string json = LocalStorage.GetValue("quiz", "");
            _task = JsonConvert.DeserializeObject<Task>(json);
            _response = new TaskResponse { Responses = new QuestionResponse[_task.Questions.Length] };
            
            if(_task.Questions.Length > 0) LoadQuestion(0, _task.Questions[0]);

            player.LoadEntity("Username", 1);
            enemy.LoadEntity(_task.Title[..Math.Min(15, _task.Title.Length)], 2);
        }
        
        private void LoadQuestion(int index, Question question)
        {
            if(question == null) return;
            contentObj.SetActive(true);

            questionText.text = $"{index+1}. {question.Description}";
            gradeText.text = $"Grade: {question.MaxGrade}";
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

            _maxGrade += question.MaxGrade;
            
            if (answer.Correct)
            {
                player.Attack();
                _grade += question.MaxGrade;
                _correct++;
            }
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
            else
            {
                StartCoroutine(GameOver());
            }
        }

        private IEnumerator GameOver()
        {
            if (_grade >= _task.Questions.Length / 2)
            {
                player.Win();
                enemy.Lose();
            }
            else
            {
                enemy.Win();
                player.Lose();
            }
            yield return new WaitForSeconds(1f);
            
            gameOverController.GameOver(new Summary{
                Grade=_grade, MaxGrade = _maxGrade, Correct = _correct, 
                MaxCorrect = _task.Questions.Length, Title = _task.Title, MinWinGrade = _task.Questions.Length/2, 
                Username="Name"});
        }
    }
}