using Global.Localization;
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

        [SerializeField] private GameObject footer;

        public void GameOver(Summary summary)
        {
            question.SetActive(false);
            
            result.SetActive(true);

            if (summary.Answered != summary.Questions)
            {
                titleText.text = LocalizationManager.GetWordByKey("notAll");
                summaryText.text = LocalizationManager.GetWordByKey("notAllDescription");
                
                gradeText.text = "";
                correctText.text = "";
                footer.SetActive(false);
                
                return;
            }
            
            gradeText.text = $"{LocalizationManager.GetWordByKey("grade")} - {summary.Grade}/{summary.MaxGrade}";
            correctText.text = $"{LocalizationManager.GetWordByKey("correct")} - {summary.Correct}/{summary.Questions}";

            summaryText.text = summary.Grade >= summary.MinWinGrade
                ? $"{LocalizationManager.GetWordByKey("congratulations")}, {summary.Username}, " +
                  $"{LocalizationManager.GetWordByKey("passed")} '{summary.Title}'"
                : $"{LocalizationManager.GetWordByKey("sorry")}, {summary.Username}, " +
                  $"{LocalizationManager.GetWordByKey("unpassed")} '{summary.Title}'";

            titleText.text = summary.Title;
        }
    }
}