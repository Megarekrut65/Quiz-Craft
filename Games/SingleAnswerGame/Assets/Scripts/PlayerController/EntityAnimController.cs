using Global.Sound;
using UnityEngine;

namespace PlayerController
{
    public class EntityAnimController : MonoBehaviour
    {
        private static readonly int Weapon = Animator.StringToHash("Weapon");
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int Damage = Animator.StringToHash("TakeDamage");

        private static readonly int LoseTrigger = Animator.StringToHash("Lose");
        private static readonly int WinTrigger = Animator.StringToHash("Win");
        [SerializeField] private EntityAnimController enemy;

        [SerializeField] private GameObject[] weapons;
        [SerializeField] private Animator animator;

        [SerializeField] private int selectedWeapon;

        public void SelectWeapon(int weapon)
        {
            selectedWeapon = weapon;
        }

        public void Attack()
        {
            animator.SetInteger(Weapon, selectedWeapon);
            animator.SetTrigger(AttackTrigger);
        }

        public void Lose()
        {
            animator.SetTrigger(LoseTrigger);
        }

        public void Win()
        {
            animator.SetTrigger(WinTrigger);
        }

        private void AttackAnim()
        {
            ChangeWeapon(selectedWeapon);
        }

        private void AttackEnemy()
        {
            enemy.TakeDamage();
        }

        private void AttackEndAnim()
        {
            ChangeWeapon(-1);
        }

        private void TakeDamage()
        {
            SoundManager.PlaySound(2);
            animator.SetTrigger(Damage);
        }

        private void ChangeWeapon(int weapon)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(weapon == i);
            }
        }
    }
}