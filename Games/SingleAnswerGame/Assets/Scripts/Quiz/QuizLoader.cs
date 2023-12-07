using System;
using System.Collections;
using Global;
using Global.Data;
using Global.Localization;
using JetBrains.Annotations;
using Main;
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

        [SerializeField] private ErrorLogger logger;
        
        private int _answered;

        private int _correct;
        private Game _game;
        private GameResponse _response;

        private int _grade;
        private int _maxGrade;

        private ResponseSender _sender;
        private Task _task;
        private string _username;

        private void Start()
        {
            string token = LocalStorage.GetValue("token", "");
            string uid = LocalStorage.GetValue("uid", "");

            _sender = new ResponseSender(uid, token);

            _username = LocalStorage.GetValue("username", "unknown");
            string gameJson = LocalStorage.GetValue("quiz", "");

            _game = JsonConvert.DeserializeObject<Game>(gameJson);
            _task = _game.Task;

            string responseJson = LocalStorage.GetValue("response", "");
            _response = JsonConvert.DeserializeObject<GameResponse>(responseJson);

            contentObj.SetActive(false);
            questionText.text = "";
            gradeText.text = "";
            
            if (_task.Questions.Length > 0) LoadQuestion(0);

            player.LoadEntity(_username, WeaponConverter.Convert(_game.PlayerWeapon));
            enemy.LoadEntity(_task.Title[..Math.Min(25, _task.Title.Length)],
                WeaponConverter.Convert(_game.EnemyWeapon));
        }

        private bool AlreadyPassed(Question question)
        {
            if (question == null || _response?.Responses == null) return false;
            
            foreach (QuestionResponse response in _response.Responses)
            {
                if (response.Question != question.Id) continue;

                _answered++;
                if (response.Correct)
                {
                    _correct++;
                    _grade += question.MaxGrade;
                }

                _maxGrade += question.MaxGrade;

                return true;
            }

            return false;
        }
        
        private void LoadQuestion(int index)
        {
            Question question = null;

            do
            {
                question = _task.Questions[index];
                index++;
            } while (index < _task.Questions.Length && AlreadyPassed(question));

            if (question == null || AlreadyPassed(question) && index >= _task.Questions.Length)
            {
                StartCoroutine(GameOver());
                return;
            }

            index--;
            
            contentObj.SetActive(true);

            questionText.text = $"{index + 1}. {question.Description}";
            gradeText.text = $"{LocalizationManager.GetWordByKey("grade")}: {question.MaxGrade}";
            LoadAnswers(index, question.Answers);
        }

        private void LoadAnswers(int questionIndex, [CanBeNull] Answer[] answers)
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            if (answers == null) return;

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
            
            _sender.Send(this, question.Id, answer.Id, 
                (error, json) => LoadResponse(error, questionIndex, json));
        }

        private void LoadResponse([CanBeNull] Error error, int questionIndex, string json)
        {
            if (error != null)
            {
                Debug.Log(error.Detail);
                logger.ShowError(LocalizationManager.GetWordByKey("error"));
                return;
            }
            
            _answered++;
            ResponseAnswer responseAnswer = JsonConvert.DeserializeObject<ResponseAnswer>(json);

            if (responseAnswer == null)
            {
                logger.ShowError(LocalizationManager.GetWordByKey("error"));
                return;
            }
            
            if (responseAnswer.Correct)
            {
                player.Attack();
                _grade += _task.Questions[questionIndex].MaxGrade;
                _correct++;
            }
            else enemy.Attack();
            
            StartCoroutine(LoadNext(questionIndex + 1));
        }

        private IEnumerator LoadNext(int next)
        {
            yield return new WaitForSeconds(4.8f);

            if (next < _task.Questions.Length)
            {
                LoadQuestion(next);
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

            gameOverController.GameOver(new Summary
            {
                Answered = _answered,
                Grade = _grade, MaxGrade = _maxGrade, Correct = _correct,
                Questions = _task.Questions.Length, Title = _task.Title, MinWinGrade = _game.MinWinGrade,
                Username = _username
            });
        }
    }
}