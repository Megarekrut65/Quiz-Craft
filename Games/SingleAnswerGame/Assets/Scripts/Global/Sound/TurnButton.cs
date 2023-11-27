using System;
using UnityEngine;
using UnityEngine.UI;

namespace Global.Sound
{
    public abstract class TurnButton : MonoBehaviour
    {
        [SerializeField] private Image img;
        
        [SerializeField] private Sprite turnOn;
        [SerializeField] private Sprite turnOff;

        [SerializeField] private string key;
        
        public void Turn()
        {
            bool state = LocalStorage.GetValue(key, true);
            state = !state;
            LocalStorage.SetValue(key, state);

            Change(state);
        }

        private void Change(bool state)
        {
            img.sprite = state ? turnOn : turnOff;
            if(state) TurnOn();
            else TurnOff();
        }

        protected abstract void TurnOn();
        protected abstract void TurnOff();

        private void Start()
        {
            bool state = LocalStorage.GetValue(key, true);
            Change(state);
        }
    }
}