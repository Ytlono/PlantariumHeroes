using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class ZoneCollider : MonoBehaviour
    {
        private UltimateZoneAttack ultimateZoneAttack;
        private List<Enemy> enemiesInZone = new List<Enemy>();
        private Dictionary<Enemy, Coroutine> enemyDamageCoroutines = new Dictionary<Enemy, Coroutine>();

        public UltimateZoneAttack UltimateZoneAttack
        {
            get { return ultimateZoneAttack; }
            private set { ultimateZoneAttack = value; }
        }

        public List<Enemy> EnemiesInZone
        {
            get { return enemiesInZone; }
        }

        public Dictionary<Enemy, Coroutine> EnemyDamageCoroutines
        {
            get { return enemyDamageCoroutines; }
        }

        private void Start()
        {
            ultimateZoneAttack = FindAnyObjectByType<UltimateZoneAttack>();
            if (ultimateZoneAttack == null)
            {
                Debug.LogError("Не найден компонент UltimateZoneAttack!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;

            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null && !enemiesInZone.Contains(enemy))
                {
                    enemiesInZone.Add(enemy);
                    enemy.PoisonDamage(ultimateZoneAttack.UltimateDamage);

                    Coroutine damageCoroutine = StartCoroutine(DamageEnemyOverTime(enemy));
                    enemyDamageCoroutines[enemy] = damageCoroutine;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger)
                return;

            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null && enemiesInZone.Contains(enemy))
                {
                    if (enemyDamageCoroutines.ContainsKey(enemy))
                    {
                        StopCoroutine(enemyDamageCoroutines[enemy]);
                        enemyDamageCoroutines.Remove(enemy);
                    }

                    enemiesInZone.Remove(enemy);
                }
            }
        }

        private IEnumerator DamageEnemyOverTime(Enemy enemy)
        {
            while (true)
            {
                yield return new WaitForSeconds(ultimateZoneAttack.DamageInterval);

                if (enemy != null)
                {
                    enemy.PoisonDamage(ultimateZoneAttack.UltimateDamage);
                }
                else
                {
                    break;
                }
            }
        }

        private void OnDisable()
        {
            foreach (var coroutine in enemyDamageCoroutines.Values)
            {
                StopCoroutine(coroutine);
            }

            enemiesInZone.Clear();
            enemyDamageCoroutines.Clear();
        }
    }
}
