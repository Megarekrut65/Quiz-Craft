using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerController
{
    public class EntityAnimController : MonoBehaviour
    {
        [SerializeField] private EntityAnimController enemy;
        
        [SerializeField] private GameObject[] weapons;
        [SerializeField] private Animator animator;
        
        private static readonly int Weapon = Animator.StringToHash("Weapon");
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int Damage = Animator.StringToHash("TakeDamage");

        [SerializeField]
        private int selectedWeapon = 0;

        public void SelectWeapon(int weapon)
        {
            selectedWeapon = weapon;
        }

        public void Attack()
        {
            animator.SetInteger(Weapon, selectedWeapon);
            animator.SetTrigger(AttackTrigger);
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
