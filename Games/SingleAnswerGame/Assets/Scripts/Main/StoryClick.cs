using Global;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class StoryClick : MonoBehaviour
    {
        [SerializeField] private string scene;
        [SerializeField] private string storyId;
        [SerializeField] private string language;

        public void Click()
        {
            LocalStorage.SetValue("storyId", storyId);
            LocalStorage.SetValue("language", language);
            
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}