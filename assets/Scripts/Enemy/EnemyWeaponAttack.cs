using UnityEngine;

namespace MyGameProject
{
    public class EnemyWeaponAttack : MonoBehaviour
    {
        [SerializeField] private float weaponDamage = 10f;
        private EnemyController enemyController;
        private Collider weaponCollider;

        public float WeaponDamage
        {
            get { return weaponDamage; }
            set { weaponDamage = value; }
        }

        public EnemyController EnemyController
        {
            get { return enemyController; }
            set { enemyController = value; }
        }

        public Collider WeaponCollider
        {
            get { return weaponCollider; }
            set { weaponCollider = value; }
        }

        void Start()
        {
            EnemyController = GetComponentInParent<EnemyController>();
            WeaponCollider = GetComponentInParent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!EnemyController.Enemy.IsAlive())
            {
                WeaponCollider.enabled = false;
                return;
            }

            if (other.CompareTag("Player"))
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(WeaponDamage);
                }
            }
            else if (other.CompareTag("Statue"))
            {
                StatueController statue = other.GetComponent<StatueController>();
                if (statue != null)
                {
                    statue.TakeDamage(WeaponDamage);
                }
            }
        }
    }
}
