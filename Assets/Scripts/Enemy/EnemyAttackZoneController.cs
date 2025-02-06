using UnityEngine;

namespace MyGameProject
{
    public class EnemyAttackZoneController : MonoBehaviour
    {
        private EnemyController enemyController;
        private Collider attackZoneCollider;

        [SerializeField] private string statueTag = "Statue";

        private Transform statueTransform;
        private bool playerInZone = false;
        private bool statueInZone = false;

        public EnemyController EnemyController
        {
            get => enemyController;
            private set => enemyController = value;
        }

        public Collider AttackZoneCollider
        {
            get => attackZoneCollider;
            private set => attackZoneCollider = value;
        }

        public Transform StatueTransform
        {
            get => statueTransform;
            private set => statueTransform = value;
        }

        public bool PlayerInZone
        {
            get => playerInZone;
            private set => playerInZone = value;
        }

        public bool StatueInZone
        {
            get => statueInZone;
            private set => statueInZone = value;
        }

        private void Awake()
        {
            EnemyController = GetComponentInParent<EnemyController>();
            AttackZoneCollider = GetComponent<Collider>();

            GameObject statue = GameObject.FindWithTag(statueTag);
            if (statue != null)
            {
                StatueTransform = statue.transform;
            }
        }

        private void HandleTrigger(Collider other, bool isEntering)
        {
            if (!EnemyController.Enemy.IsAlive())
            {
                AttackZoneCollider.enabled = false;
                return;
            }

            if (other.CompareTag("Player"))
            {
                PlayerInZone = isEntering;
            }
            else if (StatueTransform != null && other.transform == StatueTransform)
            {
                StatueInZone = isEntering;
            }

            EnemyController.SetPlayerInAttackZone(PlayerInZone || StatueInZone);
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleTrigger(other, true);
        }

        private void OnTriggerStay(Collider other)
        {
            HandleTrigger(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            HandleTrigger(other, false);
        }
    }
}
