using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class QuickMenuButtons : MonoBehaviour
    {
        [SerializeField]
        private Button volumeButton;

        private Clicked clickedVolumeButton;

        private MainMenu mainMenu;

        private StopGame stopGame;

        private SoundBackground soundBackground;

        public Button VolumeButton
        {
            get { return volumeButton; }
            set { volumeButton = value; }
        }

        public Clicked ClickedVolumeButton
        {
            get { return clickedVolumeButton; }
            set { clickedVolumeButton = value; }
        }

        public MainMenu MainMenu
        {
            get { return mainMenu; }
            private set { mainMenu = value; }
        }

        public StopGame StopGame
        {
            get { return stopGame; }
            private set { stopGame = value; }
        }

        public SoundBackground SoundBackground
        {
            get { return soundBackground; }
            private set { soundBackground = value; }
        }

        private void Awake()
        {
            ClickedVolumeButton = Clicked.OFF;
            MainMenu = FindAnyObjectByType<MainMenu>();
            StopGame = FindAnyObjectByType<StopGame>();
            SoundBackground = FindAnyObjectByType<SoundBackground>();
        }

        public void OnClickPauseButton()
        {
            StopGame.StopGameProcesses(true);
            if (!MainMenu.IsMenuActive())
            {
                MainMenu.ToggleMenu();
            }
        }

        public void OnClickVolumeButton()
        {
            if (!IsClick(ClickedVolumeButton))
            {
                Debug.Log("PRESSED");
                SoundBackground.OffAll();
                VolumeButton.image.color = Color.grey;
                ClickedVolumeButton = Clicked.ON;
            }
            else
            {
                Debug.Log("NOTPRESSED");
                SoundBackground.OnAll();
                VolumeButton.image.color = Color.white;
                ClickedVolumeButton = Clicked.OFF;
            }
        }

        private bool IsClick(Clicked buttonClick)
        {
            return buttonClick == Clicked.ON;
        }
    }
}
