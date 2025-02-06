using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class ShopPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject shopPanel;
        [SerializeField]
        private ActiveState shopState;

        [SerializeField]
        private GameObject skinShop;
        [SerializeField]
        private GameObject upgradeShop;

        public GameObject ShopPanelUI
        {
            get { return shopPanel; }
            set { shopPanel = value; }
        }

        public ActiveState ShopState
        {
            get { return shopState; }
            set { shopState = value; }
        }

        public GameObject SkinShop
        {
            get { return skinShop; }
            set { skinShop = value; }
        }

        public GameObject UpgradeShop
        {
            get { return upgradeShop; }
            set { upgradeShop = value; }
        }

        private void Start()
        {
            ShopPanelUI.SetActive(false);
            ShopState = ActiveState.OFF;
        }

        public void ToggleShopPanel()
        {
            if (!IsShopActive())
            {
                SetShopActive(true);
                ShopState = ActiveState.ON;
                Debug.Log("Menu turned ON");
            }
            else
            {
                SetShopActive(false);
                ShopState = ActiveState.OFF;
                Debug.Log("Menu turned OFF");
            }
        }

        public bool IsShopActive()
        {
            return ShopState == ActiveState.ON;
        }

        public void SetShopActive(bool isActive)
        {
            ShopPanelUI.SetActive(isActive);
        }

        private void SwitchToSkinShop()
        {
            UpgradeShop.SetActive(false);
            SkinShop.SetActive(true);
        }

        private void SwitchToUpgradeShop()
        {
            SkinShop.SetActive(false);
            UpgradeShop.SetActive(true);
        }

        public void OnClickUpgradeButton()
        {
            SwitchToUpgradeShop();
        }

        public void OnClickSkinsButton()
        {
            SwitchToSkinShop();
        }
    }
}
