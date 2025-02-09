using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MyGameProject
{
    public class Enemy : Character
    {
        private NavMeshAgent agent;
        private Transform target;
        private Animator animator;
        private Collider attackZoneCollider;
        private EnemyController enemyController;

        [SerializeField] private float attackDamage;

        public NavMeshAgent Agent
        {
            get => agent;
            set => agent = value;
        }

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        public Animator Animator
        {
            get => animator;
            set => animator = value;
        }

        public Collider AttackZoneCollider
        {
            get => attackZoneCollider;
            private set => attackZoneCollider = value;
        }

        public float AttackDamage
        {
            get => attackDamage;
            set => attackDamage = value;
        }

        private void Awake()
        {
            MaxHealth = 100f;
            Agent = GetComponent<NavMeshAgent>();
            Target = FindObjectOfType<Player>().transform;
            Animator = GetComponent<Animator>();
            enemyController = GetComponent<EnemyController>();
            CurrentHealth = MaxHealth;
            FindAttackZone();
        }

        public void ChangeTarget(Transform newTarget)
        {
            Target = newTarget;
        }

        public override void Heal(float healingAmount)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + healingAmount, MaxHealth);
            UpdateState();
        }

        public override void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            UpdateState();
            enemyController.TakeDamage();
        }

        public void PoisonDamage(float damage)
        {
            CurrentHealth -= damage;
            UpdateState();
        }

        public override void UpdateState()
        {
            StateSetGet = CurrentHealth <= 0 ? State.DEAD : State.INGAME;
        }

        public float GetDamageForAttack()
        {
            return AttackDamage;
        }

        private void FindAttackZone()
        {
            GameObject attackZoneObject = GameObject.FindWithTag("AttackZone");

            if (attackZoneObject != null)
            {
                AttackZoneCollider = attackZoneObject.GetComponent<Collider>();

                if (AttackZoneCollider == null)
                {
                    Debug.LogError("На объекте AttackZone нет коллайдера!");
                }
            }
            else
            {
                Debug.LogError("Объект с тегом AttackZone не найден!");
            }
        }
    }
}
