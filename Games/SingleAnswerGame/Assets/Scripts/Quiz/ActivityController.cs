using UnityEngine;

namespace Quiz
{
    public class ActivityController : MonoBehaviour
    {
        [SerializeField] private GameObject obj;

        public void Activity()
        {
            obj.SetActive(!obj.activeInHierarchy);
        }
    }
}