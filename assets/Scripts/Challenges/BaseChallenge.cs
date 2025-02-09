using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public abstract class BaseChallenge : MonoBehaviour
    {
        public enum ChallengeState
        {
            NotStarted,
            InProgress,
            Completed
        }

        public enum CompletedState
        {
            UNKNOWN,
            WON,
            LOSE
        }

        public enum DifficultyLevel
        {
            Easy,
            Medium,
            Hard
        }

        public ChallengeState CurrentState { get; protected set; } = ChallengeState.NotStarted;
        public CompletedState Completed { get; protected set; } = CompletedState.LOSE;
        public DifficultyLevel CurrentDifficulty { get; protected set; }
        [SerializeField] private Text completionMessage;
        public Text CompletionMessage {
            get { return completionMessage; }
            protected set { completionMessage = value; } 
        }

        public abstract void StartLevelOne();
        public abstract void StartLevelTwo();
        public abstract void StartLevelThree();
        protected abstract void EndChallenge();

        protected void NotifyChallengeStarted(string level)
        {
            if (CurrentState == ChallengeState.NotStarted)
            {
                CurrentState = ChallengeState.InProgress;
                Debug.Log($"Челлендж начат: {level}");
            }
        }

        public void CompleteChallenge()
        {
            if (CurrentState == ChallengeState.InProgress)
            {
                CurrentState = ChallengeState.Completed;
                Debug.Log("Челлендж завершен!");
            }
        }

        public bool IsCompleted()
        {
            return CurrentState == ChallengeState.Completed;
        }

        public bool IsStarted()
        {
            return CurrentState != ChallengeState.NotStarted;
        }

        public void SwitchStart()
        {
            if (IsStarted())
            {
                CurrentState = ChallengeState.NotStarted;
                Completed = CompletedState.UNKNOWN;
            }
            else
            {
                CurrentState = ChallengeState.InProgress;
            }
        }

        public bool IsWon()
        {
            return Completed == CompletedState.WON;
        }

        public void GetReward(Player player)
        {
            if (IsWon())
            {
                int rewardAmount = CalculateReward();
                player.Money += rewardAmount;

                Debug.Log($"Игрок получил награду: {rewardAmount} денег. Общая сумма: {player.Money}");
            }
        }

        protected int CalculateReward()
        {
            int rewardAmount = 0;

            switch (CurrentDifficulty)
            {
                case DifficultyLevel.Easy:
                    rewardAmount = 200;
                    break;
                case DifficultyLevel.Medium:
                    rewardAmount = 500;
                    break;
                case DifficultyLevel.Hard:
                    rewardAmount = 1000;
                    break;
            }

            return rewardAmount;
        }


        protected void DisplayCompletionMessage()
        {
            if (CompletionMessage != null)
            {
                CompletionMessage.text = IsWon()
                    ? $"Challenge completed! Reward: {CalculateReward()}!"
                    : "Challenge failed!";
                CompletionMessage.gameObject.SetActive(true);

                Invoke(nameof(HideCompletionMessage), 3f);
            }
        }

        private void HideCompletionMessage()
        {
            if (CompletionMessage != null)
            {
                CompletionMessage.gameObject.SetActive(false);
            }
        }
    }
}
