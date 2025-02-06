using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Enemy enemy;

        public Image HealthBar
        {
            get { return healthBar; }
            set { healthBar = value; }
        }

        public Enemy Enemy
        {
            get { return enemy; }
            set { enemy = value; }
        }

        public void Initialize(Enemy enemy)
        {
            Enemy = enemy;
            UpdateHealthBar();
        }

        void Update()
        {
            if (Enemy != null && HealthBar != null)
            {
                UpdateHealthBar();
            }
        }

        private void UpdateHealthBar()
        {
            HealthBar.fillAmount = Enemy.CurrentHealth / Enemy.MaxHealth;
        }
    }
}
