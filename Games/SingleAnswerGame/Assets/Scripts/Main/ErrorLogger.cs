using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class ErrorLogger : MonoBehaviour
    {
        [SerializeField] private Text text;

        public void ShowError(string error)
        {
            text.text = error;
            gameObject.SetActive(true);
        }
    }
}