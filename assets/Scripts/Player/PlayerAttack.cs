using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class PlayerAttack : MonoBehaviour
    {
        private Animator animator;
        private WeaponAttack weaponAttack;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            weaponAttack = GetComponentInChildren<WeaponAttack>();

            if (weaponAttack == null)
            {
                Debug.LogWarning("WeaponAttack не найден на дочернем объекте.");
            }
        }

        public void Attack1(bool isAttack1)
        {
            animator.SetBool("IsAttack1", isAttack1);

            if (weaponAttack != null)
            {
                weaponAttack.IsEnableWeaponCollider(isAttack1);
            }
            else
            {
                Debug.LogWarning("WeaponAttack отсутствует, невозможно включить коллайдер.");
            }
        }

        public void Attack2()
        {
            
        }

        public void Attack3()
        {
        }

        public void AttackInJump()
        {
        }

        public bool CheckEndOfAnimation()
        {
            var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName("Attack1") && animatorStateInfo.normalizedTime >= 1f)
            {
                Debug.Log("Анимация завершена");
                weaponAttack.ResetDamageEnemies();
                return true;
            }

            return false;
        }
    }
}
