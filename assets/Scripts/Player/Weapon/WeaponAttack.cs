using MyGameProject;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class WeaponAttack : MonoBehaviour
    {
        private Collider weaponCollider;
        private List<Collider> damagedEnemies = new List<Collider>();

        [SerializeField]
        private Weapon weapon;

        public Collider WeaponCollider
        {
            get { return weaponCollider; }
            private set { weaponCollider = value; }
        }

        public List<Collider> DamagedEnemies
        {
            get { return damagedEnemies; }
        }

        public Weapon Weapon
        {
            get { return weapon; }
            private set { weapon = value; }
        }

        void Start()
        {
            InitializeWeaponCollider();
            FindWeaponIfNotAssigned();
        }

        private void InitializeWeaponCollider()
        {
            weaponCollider = GetComponent<Collider>();

            if (weaponCollider != null)
            {
                weaponCollider.isTrigger = true;
                weaponCollider.enabled = false;
            }
        }

        private void FindWeaponIfNotAssigned()
        {
            if (weapon == null)
            {
                weapon = FindObjectOfType<Weapon>();
                if (weapon == null)
                {
                    Debug.LogError("Weapon not assigned or found!");
                }
            }
        }

        public void IsEnableWeaponCollider(bool isEnable)
        {
            if (weaponCollider != null)
            {
                weaponCollider.enabled = isEnable;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;

            HandleEnemyCollision(other);
        }

        private void HandleEnemyCollision(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !damagedEnemies.Contains(other))
            {
                Enemy enemy = other.GetComponent<Enemy>();

                if (enemy != null)
                {
                    ApplyDamageToEnemy(enemy);
                    AddEnemyToDamagedList(other);
                    IncreasePlayerManna(enemy);
                }
            }
        }

        private void ApplyDamageToEnemy(Enemy enemy)
        {
            float damageAmount = weapon.Damage;
            enemy.TakeDamage(damageAmount);
        }

        private void AddEnemyToDamagedList(Collider enemyCollider)
        {
            damagedEnemies.Add(enemyCollider);
        }

        private void IncreasePlayerManna(Enemy enemy)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                float damageAmount = weapon.Damage;
                Debug.Log($"Increasing manna by {damageAmount * 0.05f} from enemy damage: {damageAmount}");
                player.IncreaseManna(damageAmount * 0.05f);
            }
        }


        public void ResetDamageEnemies()
        {
            damagedEnemies.Clear();
        }

        public float GetDamageAmount()
        {
            return weapon != null ? weapon.Damage : 0f;
        }
    }
}
