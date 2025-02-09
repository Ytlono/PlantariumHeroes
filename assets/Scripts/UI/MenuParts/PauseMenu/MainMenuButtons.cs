using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField]
        private GameObject confirmExitPanel;

        private MainMenu mainMenu;

        public GameObject ConfirmExitPanel
        {
            get { return confirmExitPanel; }
            set { confirmExitPanel = value; }
        }

        public MainMenu MainMenu
        {
            get { return mainMenu; }
            set { mainMenu = value; }
        }

        private void Awake()
        {
            MainMenu = FindAnyObjectByType<MainMenu>();
        }

        public void ConfirmExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            Debug.Log("Game is exiting...");
        }

        public void CancelExit()
        {
            ConfirmExitPanel.SetActive(false);
            MainMenu.ToggleMenu();
        }
    }
}
