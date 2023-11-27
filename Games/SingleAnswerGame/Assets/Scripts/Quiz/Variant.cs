using System;
using Global.Localization;
using Global.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz
{
    public class Variant: MonoBehaviour
    {
        [SerializeField]
        private AnswerClick answerClick;

        [SerializeField] private Text text;

        public void SetData(int index, Answer answer, Action<int, Answer> click)
        {
            answerClick.Click = () => click(index, answer);

            text.text = answer.Option;
        } 
    }
}