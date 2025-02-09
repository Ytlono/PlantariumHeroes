using MyGameProject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainMenu;

        private ActiveState menuState;
        private MainMenuButtons mainMenuButtons;
        private ShopPanel shopPanel;
        private SettingsPanel settingsPanel;
        private HelpPanel helpPanel;
        private StopGame stopGame;

        public ActiveState MenuState
        {
            get { return menuState; }
            private set { menuState = value; }
        }

        private void Awake()
        {
            mainMenuButtons = FindAnyObjectByType<MainMenuButtons>();
            shopPanel = FindAnyObjectByType<ShopPanel>();
            settingsPanel = FindAnyObjectByType<SettingsPanel>();
            helpPanel = FindAnyObjectByType<HelpPanel>();
            stopGame = FindAnyObjectByType<StopGame>();

            mainMenu.SetActive(false);
            menuState = ActiveState.OFF;
        }

        public void ToggleMenu()
        {
            if (!IsMenuActive())
            {
                SetMenuActive(true);
                menuState = ActiveState.ON;
                Debug.Log("Menu turned ON");
            }
            else
            {
                SetMenuActive(false);
                menuState = ActiveState.OFF;
                Debug.Log("Menu turned OFF");
            }
        }

        public void SetMenuActive(bool isActive)
        {
            mainMenu.SetActive(isActive);
        }

        public bool IsMenuActive()
        {
            return menuState == ActiveState.ON;
        }

        public void OnClickResumeButton()
        {
            stopGame.StopGameProcesses(false);
            ToggleMenu();
        }

        public void OnClickExitButton()
        {
            ToggleMenu();
            mainMenuButtons.ConfirmExitPanel.SetActive(true);
        }

        public void OnClickShopButton()
        {
            shopPanel.ToggleShopPanel();
        }

        public void OnClickShopButtonReturn()
        {
            shopPanel.ToggleShopPanel();
        }

        public void OnClickSettingsButton()
        {
            settingsPanel.ToggleShopPanel();
        }

        public void OnClickSettingsButtonReturn()
        {
            settingsPanel.ToggleShopPanel();
        }

        public void OnClickHelpButton()
        {
            helpPanel.ToggleShopPanel();
        }

        public void OnClickHelpButtonReturn()
        {
            helpPanel.ToggleShopPanel();
        }
    }
}
