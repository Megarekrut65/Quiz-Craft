using UnityEngine;
using UnityEngine.UI;

namespace PlayerController
{
    public class EntityController : MonoBehaviour
    {
        [SerializeField] private EntityAnimController animController;

        [SerializeField] private Text nicknameText;

        public void LoadEntity(string nickname, int weapon)
        {
            animController.SelectWeapon(weapon);
            nicknameText.text = nickname;
        }

        public void Lose()
        {
            animController.Lose();
        }

        public void Win()
        {
            animController.Win();
        }
    }
}