using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class UpgradeLevels
    {
        private Image levelBar;
        private Text levelText;
        private Text price;
        private int maxLevel;
        private int currentLevel;
        public Image LevelBar
        {
            get { return levelBar; }
            private set { levelBar = value; }
        }

        public Text LevelText
        {
            get { return levelText; }
            private set { levelText = value; }
        }

        public Text Price
        {
            get { return price; }
            private set { price = value; }
        }

        public int MaxLevel
        {
            get { return maxLevel; }
            private set { maxLevel = value; }
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        public  UpgradeLevels(Image levelBar, Text levelText, Text price, int maxLevel,int currentLevel)
        {
            LevelBar = levelBar;
            LevelText = levelText;
            Price = price;
            MaxLevel = maxLevel;
            CurrentLevel = currentLevel;
            SetPrice(100);
        }

        public void SetLevel(int level)
        {
            if (LevelText != null)
            {
                LevelText.text = $"{level}";
            }
        }

        public void SetPrice(int price)
        {
            if (Price != null)
            {
                Price.text = $"{price}";
            }
        }

        public void SetBarFill(float fillAmount)
        {
            if (LevelBar != null)
            {
                LevelBar.fillAmount = fillAmount;
            }
        }

    }
}
