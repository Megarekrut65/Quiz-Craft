using System;
using Global.Localization;
using Story.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Story
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