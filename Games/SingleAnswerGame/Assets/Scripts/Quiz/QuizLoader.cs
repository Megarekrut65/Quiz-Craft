
using System;
using System.Collections;
using Global;
using Global.Data;
using Global.Localization;
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

        private ResponseSender _sender;
        private Game _game;
        private Task _task;
        private string _username;

        private int _grade = 0;
        private int _maxGrade = 0;
        
        private int _correct = 0;
        private int _answered = 0;

        private void Start()
        {
            string token = LocalStorage.GetValue("token", "");
            string uid = LocalStorage.GetValue("uid", "");

            _sender = new ResponseSender(uid, token);
                
            _username = LocalStorage.GetValue("username", "unknown");
            string json = LocalStorage.GetValue("quiz", "");

            _game = JsonConvert.DeserializeObject<Game>(json);
            _task = _game.Task;

            if(_task.Questions.Length > 0) LoadQuestion(0, _task.Questions[0]);

            player.LoadEntity(_username, WeaponConverter.Convert(_game.PlayerWeapon));
            enemy.LoadEntity(_task.Title[..Math.Min(15, _task.Title.Length)], WeaponConverter.Convert(_game.EnemyWeapon));
        }
        
        private void LoadQuestion(int index, Question question)
        {
            if(question == null) return;
            contentObj.SetActive(true);

            questionText.text = $"{index+1}. {question.Description}";
            gradeText.text = $"{LocalizationManager.GetWordByKey("grade")}: {question.MaxGrade}";
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
            _maxGrade += question.MaxGrade;
            
            if (answer.Correct)
            {
                player.Attack();
                _grade += question.MaxGrade;
                _correct++;
            }
            else enemy.Attack();

            StartCoroutine(SendAnswer(questionIndex, question.Id, answer.Id));
        }

        private IEnumerator SendAnswer(int questionIndex, int question, int answer)
        {
            try
            {
                _sender.Send(question, answer);
                _answered++;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            yield return null;
            
            StartCoroutine(LoadNext(questionIndex + 1));
        }

        private IEnumerator LoadNext(int next)
        {
            yield return new WaitForSeconds(4.8f);
            
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
            if (_grade >= _game.MinWinGrade)
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
                Answered = _answered,
                Grade=_grade, MaxGrade = _maxGrade, Correct = _correct, 
                Questions = _task.Questions.Length, Title = _task.Title, MinWinGrade = _game.MinWinGrade, 
                Username=_username});
        }
    }
}