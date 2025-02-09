using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class NpsUI : MonoBehaviour
    {
        [SerializeField] private Dropdown difficultyDropdown;
        [SerializeField] private Button startButton;
        [SerializeField] private BaseChallenge challenge;

        public Dropdown DifficultyDropdown
        {
            get => difficultyDropdown;
            set => difficultyDropdown = value;
        }

        public Button StartButton
        {
            get => startButton;
            set => startButton = value;
        }

        public BaseChallenge Challenge
        {
            get => challenge;
            set => challenge = value;
        }

        private void Start()
        {
            if (Challenge == null)
            {
                Debug.LogError("Challenge не привязан!");
                return;
            }

            InitializeDropdown();
            if (StartButton != null)
            {
                StartButton.onClick.AddListener(OnStartButtonClick);
            }
        }

        private void InitializeDropdown()
        {
            if (DifficultyDropdown != null)
            {
                DifficultyDropdown.ClearOptions();
                List<string> difficulties = new List<string> { "Easy", "Medium", "Hard" };
                DifficultyDropdown.AddOptions(difficulties);
            }
        }

        private void OnStartButtonClick()
        {
            if (Challenge == null)
            {
                Debug.LogError("Challenge не назначен!");
                return;
            }

            int selectedDifficulty = DifficultyDropdown.value;
            StartChallenge(selectedDifficulty);
        }

        private void StartChallenge(int difficulty)
        {
           
            if (!Challenge.IsStarted())
            {
                switch (difficulty)
                {
                    case 0:
                        Challenge.StartLevelOne();
                        break;
                    case 1:
                        Challenge.StartLevelTwo();
                        break;
                    case 2:
                        Challenge.StartLevelThree();
                        break;
                    default:
                        Debug.LogError("Неизвестный уровень сложности!");
                        break;
                }
            }
        }
    }
}
