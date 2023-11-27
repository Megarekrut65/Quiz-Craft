using System;
using UnityEngine;

namespace Story
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