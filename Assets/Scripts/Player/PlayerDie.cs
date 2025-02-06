using UnityEngine;


namespace MyGameProject
{
    public class PlayerDie : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void ActivateDieAnimation()
        {
            animator.SetBool("IsDie", true);
        }

        public void ActivateDieRecoveryAnimation()
        {
            animator.SetBool("IsDie", false);
        }

    }
} 