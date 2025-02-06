using MyGameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MyGameProject
{
    public enum AttackZoneState
    {
        None,
        InZone,
        OutZone
    }

    public enum ActionState
    {
        None,
        Moving,
        Attacking,
        Stopped,
        TakingDamage
    }

    public class EnemyController : MonoBehaviour
    {
        private Enemy enemy;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Collider weaponCollider;

        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float rotationSpeed = 5f;
        private float attackCooldownTimer = 0f;

        private AttackZoneState attackZoneState = AttackZoneState.None;
        private ActionState actionState = ActionState.None;

        [SerializeField]
        private List<GameObject> dropList;

        public Enemy Enemy
        {
            get { return enemy; }
            set { enemy = value; }
        }

        public Animator Animator
        {
            get { return animator; }
            set { animator = value; }
        }

        public NavMeshAgent NavMeshAgent
        {
            get { return navMeshAgent; }
            set { navMeshAgent = value; }
        }

        public Collider WeaponCollider
        {
            get { return weaponCollider; }
            set { weaponCollider = value; }
        }

        public float AttackCooldown
        {
            get { return attackCooldown; }
            set { attackCooldown = value; }
        }

        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        public float AttackCooldownTimer
        {
            get { return attackCooldownTimer; }
            set { attackCooldownTimer = value; }
        }

        public AttackZoneState AttackZoneState
        {
            get { return attackZoneState; }
            set { attackZoneState = value; }
        }

        public ActionState ActionState
        {
            get { return actionState; }
            set { actionState = value; }
        }

        public List<GameObject> DropList
        {
            get { return dropList; }
            set { dropList = value; }
        }

        void Awake()
        {
            Enemy = GetComponent<Enemy>();
            Animator = GetComponent<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            WeaponCollider = GetComponentInChildren<Collider>();
        }

        void Update()
        {
            if (!Enemy.IsAlive())
            {
                DeathControl();
                return;
            }

            if (ActionState != ActionState.TakingDamage)
            {
                HandleMovement();
                UpdateAnimationParameters();
            }
        }

        private void DropItems()
        {
            if (DropList != null && DropList.Count > 0 && Random.Range(0f, 1f) <= 0.15f)
            {
                int randomIndex = Random.Range(0, DropList.Count);
                GameObject itemToDrop = DropList[randomIndex];

                Vector3 dropPosition = transform.position + new Vector3(0, 0.3f, 0);
                Instantiate(itemToDrop, dropPosition, Quaternion.identity);
            }
        }

        private void DeathControl()
        {
            if (!Enemy.IsAlive())
            {
                if (NavMeshAgent.enabled == true)
                {
                    Animator.SetBool("IsDie", true);
                    DropItems();
                    DeactivateEnemy();
                    DisableWeaponCollider();
                }
            }
        }

        public void DeactivateEnemy()
        {
            NavMeshAgent.enabled = false;
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        private void DisableWeaponCollider()
        {
            if (WeaponCollider != null)
            {
                WeaponCollider.enabled = false;
            }
        }

        private void HandleMovement()
        {
            if (Enemy.Target == null || AttackZoneState == AttackZoneState.InZone)
            {
                NavMeshAgent.isStopped = true;

                if (Enemy.Target != null)
                {
                    RotateTowardsTarget();
                }
            }
            else
            {
                NavMeshAgent.isStopped = false;
                NavMeshAgent.SetDestination(Enemy.Target.position);
            }
        }

        private void UpdateAnimationParameters()
        {
            if (NavMeshAgent.velocity.magnitude > 0.1f)
            {
                Animator.SetFloat("Speed", NavMeshAgent.velocity.magnitude);
            }
            else
            {
                Animator.SetFloat("Speed", 0);
            }
        }

        public void SetPlayerInAttackZone(bool inZone)
        {
            AttackZoneState = inZone ? AttackZoneState.InZone : AttackZoneState.OutZone;

            if (inZone)
            {
                if (!IsCooldownActive())
                {
                    StartAttack();
                }
            }
            else
            {
                StopAttack();
            }
        }

        private void StartAttack()
        {
            RotateTowardsTarget();
            ActionState = ActionState.Attacking;
            Animator.SetBool("IsAttack", true);
            StartCoroutine(AttackCooldownRoutine());
        }

        private void StopAttack()
        {
            ActionState = ActionState.None;
            Animator.SetBool("IsAttack", false);
        }

        private void RotateTowardsTarget()
        {
            if (Enemy.Target == null)
                return;

            Vector3 directionToTarget = (Enemy.Target.position - transform.position).normalized;
            directionToTarget.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
        }

        private IEnumerator AttackCooldownRoutine()
        {
            AttackCooldownTimer = AttackCooldown;
            while (AttackCooldownTimer > 0)
            {
                AttackCooldownTimer -= Time.deltaTime;
                yield return null;
            }
        }

        private bool IsCooldownActive()
        {
            return AttackCooldownTimer > 0;
        }

        public void TakeDamage()
        {
            if (ActionState == ActionState.TakingDamage)
                return;

            ActionState = ActionState.TakingDamage;
            NavMeshAgent.isStopped = true;

            Animator.SetBool("IsDamage", true);

            StartCoroutine(WaitForDamageAnimation());
        }

        private IEnumerator WaitForDamageAnimation()
        {
            yield return new WaitWhile(() =>
                Animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") &&
                Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

            Animator.SetBool("IsDamage", false);
            ActionState = ActionState.Moving;
            if (Enemy.IsAlive())
            {
                NavMeshAgent.isStopped = false;
            }
        }

        public void StopActions()
        {
            ActionState = ActionState.Stopped;
            NavMeshAgent.isStopped = true;
            Animator.SetBool("IsAttack", false);
        }

        public void ResumeActions()
        {
            ActionState = ActionState.Moving;
            NavMeshAgent.isStopped = false;
        }
    }
}
