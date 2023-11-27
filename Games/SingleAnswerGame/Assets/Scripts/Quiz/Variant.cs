using System;
using Global.Localization;
using Quiz.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz
{
    public class Variant: MonoBehaviour
    {
        [SerializeField]
        private AnswerClick answerClick;

        [SerializeField] private Text text;

        public void SetData(Answer answer, Action<string, string> click, Font font)
        {
            answerClick.Click = () => click(answer.nextFrameId, answer.action);

            text.text = LocalizationManager.GetWordByKey(answer.textKey);
            text.font = font;
        } 
    }
}