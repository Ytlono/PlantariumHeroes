using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Image healthBarBorder;
        [SerializeField] private Color colorFirst;
        [SerializeField] private Color colorSecond;
        [SerializeField] private Text hpPercentage;

        public Image HealthBarUI
        {
            get { return healthBar; }
            set { healthBar = value; }
        }

        public Image HealthBarBorder
        {
            get { return healthBarBorder; }
            set { healthBarBorder = value; }
        }

        public Color ColorFirst
        {
            get { return colorFirst; }
            set { colorFirst = value; }
        }

        public Color ColorSecond
        {
            get { return colorSecond; }
            set { colorSecond = value; }
        }

        public Text HpPercentage
        {
            get { return hpPercentage; }
            set { hpPercentage = value; }
        }

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            if (healthBar != null && healthBarBorder != null)
            {
                healthBar.fillAmount = currentHealth / maxHealth;
                healthBarBorder.color = Color.Lerp(colorFirst, colorSecond, 1 - (currentHealth / maxHealth));
                hpPercentage.text = $"{currentHealth.ToString()}/{maxHealth.ToString()}";
            }
        }
    }
}
