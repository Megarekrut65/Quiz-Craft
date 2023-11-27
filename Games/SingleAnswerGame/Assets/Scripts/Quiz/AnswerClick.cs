using System;
using UnityEngine;

namespace Quiz
{
    public class AnswerClick : MonoBehaviour
    {
        public Action Click;

        public void ClickEvent()
        {
            Click?.Invoke();
        }
    }
}