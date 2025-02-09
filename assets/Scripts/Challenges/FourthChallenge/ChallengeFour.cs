using MyGameProject;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class ChallengeFour : BaseChallenge
    {
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private GameObject GameZone;
        [SerializeField] private Vector3 spawnPosition = new Vector3(-30, 0, 35);
        private Enemy boss;

        private void Start()
        {
            if (bossPrefab == null)
            {
                Debug.LogError("Boss prefab is not assigned.");
                return;
            }

            if (GameZone != null)
            {
                GameZone.SetActive(false);
            }
        }

        public override void StartLevelOne()
        {
            CurrentDifficulty = DifficultyLevel.Easy;
            NotifyChallengeStarted("Level 1");
            SpawnBoss();
            SetBossHealth(600);
            ShowGameZone();  
        }

        public override void StartLevelTwo()
        {
            CurrentDifficulty = DifficultyLevel.Medium;
            NotifyChallengeStarted("Level 2");
            SpawnBoss();
            SetBossHealth(1200);
            ShowGameZone();  
        }

        public override void StartLevelThree()
        {
            CurrentDifficulty = DifficultyLevel.Hard;
            NotifyChallengeStarted("Level 3");
            SpawnBoss();
            SetBossHealth(2400);
            ShowGameZone(); 
        }

        private void SpawnBoss()
        {
            if (boss != null) return;

            GameObject bossInstance = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            boss = bossInstance.GetComponent<Enemy>();

            if (boss == null)
            {
                Debug.LogError("Boss prefab does not have an Enemy component.");
                Destroy(bossInstance);
                return; // Завершаем выполнение, чтобы не вызвать другие ошибки.
            }
        }


        private void SetBossHealth(int health)
        {
            if (boss != null)
            {
                boss.MaxHealth = health;
            }
        }

        private void Update()
        {
            CheckChallengeConditions();
        }


        private void CheckChallengeConditions()
        {
            if (IsStarted())
            {
               
                if (boss == null || !boss.IsAlive())
                {
                    CompleteChallenge(CompletedState.WON);
                }
                else if (FindObjectOfType<Player>()?.IsAlive() == false)
                {
                    CompleteChallenge(CompletedState.LOSE);
                }
            }
        }

        private void CompleteChallenge(CompletedState completedState)
        {
            Completed = completedState;
            CurrentState = ChallengeState.Completed;

            if (IsWon())
            {
                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    GetReward(player);
                }
            }

            DisplayCompletionMessage();
            EndChallenge();
        }

        protected override void EndChallenge()
        {
            if (boss != null)
            {
                Destroy(boss.gameObject);
                boss = null;
            }

            SwitchStart();
            HideGameZone();
        }

        private void ShowGameZone()
        {
            if (GameZone != null)
            {
                GameZone.SetActive(true);
            }
        }

        private void HideGameZone()
        {
            if (GameZone != null)
            {
                GameZone.SetActive(false);
            }
        }
    }
}
