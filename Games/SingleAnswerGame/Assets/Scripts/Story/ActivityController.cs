using UnityEngine;

namespace Story
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