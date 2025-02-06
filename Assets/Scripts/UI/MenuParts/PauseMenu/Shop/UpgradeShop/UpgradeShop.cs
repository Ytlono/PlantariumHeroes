using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MyGameProject.UpgradeShop;

namespace MyGameProject
{
    public class UpgradeShop : MonoBehaviour
    {
        [Serializable]
        public struct UpgradeUI
        {
            public Image BarImage
            {
                get => barImage;
                set => barImage = value;
            }

            public Text LevelText
            {
                get => levelText;
                set => levelText = value;
            }

            public Text PriceText
            {
                get => priceText;
                set => priceText = value;
            }

            [SerializeField] private Image barImage;
            [SerializeField] private Text levelText;
            [SerializeField] private Text priceText;
        }

        [SerializeField]
        private List<UpgradeUI> upgradeUIElements;

        private PlayerData playerData; 

        private List<UpgradeLevels> upgradeLevels;

        public List<UpgradeUI> UpgradeUIElements
        {
            get => upgradeUIElements;
            set => upgradeUIElements = value;
        }

        public List<UpgradeLevels> UpgradeLevelsList
        {
            get => upgradeLevels;
            private set => upgradeLevels = value;
        }

        public PlayerData PlayerData
        {
            get => playerData;
            private set => playerData = value;
        }

        private void Start()
        {
            PlayerData = FindObjectOfType<PlayerData>(); 

            if (PlayerData == null)
            {
                Debug.LogError("PlayerData not found!");
                return;
            }

            upgradeLevels = new List<UpgradeLevels>();

            if (UpgradeUIElements.Count != 5)
            {
                return;
            }

            upgradeLevels.Add(new UpgradeLevels(UpgradeUIElements[0].BarImage, UpgradeUIElements[0].LevelText, UpgradeUIElements[0].PriceText, 10, PlayerData.Weapon.GetCurrentLevel()));
            upgradeLevels.Add(new UpgradeLevels(UpgradeUIElements[1].BarImage, UpgradeUIElements[1].LevelText, UpgradeUIElements[1].PriceText, 5, PlayerData.Player.GetCurrentHealthLevel()));
            upgradeLevels.Add(new UpgradeLevels(UpgradeUIElements[2].BarImage, UpgradeUIElements[2].LevelText, UpgradeUIElements[2].PriceText, 10, PlayerData.UltimateZoneAttack.GetCurrentZoneLevel()));
            upgradeLevels.Add(new UpgradeLevels(UpgradeUIElements[3].BarImage, UpgradeUIElements[3].LevelText, UpgradeUIElements[3].PriceText, 5, PlayerData.Player.GetCurrentMannaRechargeLevel()));
            upgradeLevels.Add(new UpgradeLevels(UpgradeUIElements[4].BarImage, UpgradeUIElements[4].LevelText, UpgradeUIElements[4].PriceText, 10, PlayerData.PlayerUltimate.GetCurrentBoostLevel()));

            foreach (var upgradeLevel in upgradeLevels)
            {
                upgradeLevel.SetPrice((upgradeLevel.CurrentLevel + 1) * 100);
            }
            UpdateUI();
        }

        public void Upgrade(int index, float addValue, Action<float> onUpgradeAction)
        {
            if (index < 0 || index >= upgradeLevels.Count)
            {
                return;
            }

            UpgradeLevels upgrade = upgradeLevels[index];

            if (upgrade.CurrentLevel < upgrade.MaxLevel)
            {
                int price = (upgrade.CurrentLevel + 1) * 100;

                if (PlayerData.Player.MakePurchase(price))
                {
                    upgrade.CurrentLevel++;
                    upgrade.SetPrice(price);

                    onUpgradeAction?.Invoke(addValue);

                    UpdateUI();
                }
                else
                {
                    Debug.Log("Покупка не удалась. Недостаточно средств.");
                }
            }
            else
            {
                Debug.Log("Максимальный уровень достигнут.");
            }
        }

        public void OnClickWeaponUpgrade()
        {
            Upgrade(0, 2, PlayerData.Weapon.UpgradeDamage);
        }

        public void OnClickPlayerHealthUpgrade()
        {
            Upgrade(1, 100, PlayerData.Player.UpgradePlayerHealth);
        }

        public void OnClickUltimateZoneAttackUpgrade()
        {
            Upgrade(2, 5f, PlayerData.UltimateZoneAttack.UpgradeUltimateZoneDamage);
        }

        public void OnClickManaRechargeUpgrade()
        {
            Upgrade(3, 0.2f, PlayerData.Player.UpgradeMannaRechargeSpeed);
        }

        public void OnClickUltimateMultipliersUpgrade()
        {
            Upgrade(4, 0.2f, PlayerData.PlayerUltimate.UpgradeUltimateMultipliers);
        }

        private void UpdateUI()
        {
            foreach (var upgrade in upgradeLevels)
            {
                upgrade.SetLevel(upgrade.CurrentLevel);
                upgrade.SetBarFill(upgrade.CurrentLevel / (float)upgrade.MaxLevel);
            }
        }
    }
}
