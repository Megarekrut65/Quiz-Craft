using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Quiz
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private GameObject question;

        [SerializeField] private GameObject result;
        [SerializeField] private Text gradeText;
        [SerializeField] private Text correctText;

        [SerializeField] private Text summaryText;
        [SerializeField] private Text titleText;

        public void GameOver(Summary summary)
        {
            question.SetActive(false);
            
            result.SetActive(true);
            gradeText.text = $"Grade - {summary.Grade}/{summary.MaxGrade}";
            correctText.text = $"Correct answers - {summary.Correct}/{summary.MaxCorrect}";

            summaryText.text = summary.Grade >= summary.MinWinGrade
                ? $"Congratulations, {summary.Username}, you have passed the test '{summary.Title}'"
                : $"Sorry, {summary.Username}, you could not pass the test '{summary.Title}'";

            titleText.text = summary.Title;
        }
    }
}