using UnityEngine;
using UnityEngine.SceneManagement;

namespace Global
{
    public class OpenSceneContainer: MonoBehaviour
    {
        [SerializeField]
        private string sceneName;
        
        public void Open()
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}