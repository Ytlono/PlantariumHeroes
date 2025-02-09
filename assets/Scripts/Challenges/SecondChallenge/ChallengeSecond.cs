using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class ChallengeSecond : BaseChallenge
    {
        [SerializeField] private GameObject spawnPrefab;
        [SerializeField] private Transform statueTarget;
        [SerializeField] private GameObject challengeZone;
        private StatueController  statueController ;

        [SerializeField] private Vector2 fieldSize = new Vector2(15, 15);
        [SerializeField] private float minDistanceFromCenter = 8f;
        [SerializeField] private float spawnInterval = 1f;

        private List<Enemy> spawnedEnemies = new List<Enemy>();
        private Player player;
        private float statueHealth;

        public GameObject SpawnPrefab
        {
            get { return spawnPrefab; }
            set { spawnPrefab = value; }
        }

        public Transform StatueTarget
        {
            get { return statueTarget; }
            set { statueTarget = value; }
        }

        public Vector2 FieldSize
        {
            get { return fieldSize; }
            set { fieldSize = value; }
        }

        public float MinDistanceFromCenter
        {
            get { return minDistanceFromCenter; }
            set { minDistanceFromCenter = value; }
        }

        public float SpawnInterval
        {
            get { return spawnInterval; }
            set { spawnInterval = value; }
        }

        public List<Enemy> SpawnedEnemies
        {
            get { return spawnedEnemies; }
            set { spawnedEnemies = value; }
        }

        public Player Player
        {
            get { return player; }
            private set { player = value; }
        }

        public float StatueHealth
        {
            get { return statueHealth; }
            set { statueHealth = value; }
        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
            statueController = FindObjectOfType<StatueController>();
            statueHealth = statueController.CurrentHealth;
            statueTarget.gameObject.SetActive(false);
            challengeZone.SetActive(false);
        }

        private void Update()
        {
            statueHealth = statueController.CurrentHealth;
            CheckChallengeConditions();
        }

        public override void StartLevelOne()
        {
            CurrentDifficulty = DifficultyLevel.Easy;
            statueTarget.gameObject.SetActive(true);
            NotifyChallengeStarted("Level 1");
            StartCoroutine(SpawnEnemiesRoutine(20));
        }

        public override void StartLevelTwo()
        {
            CurrentDifficulty = DifficultyLevel.Medium;
            statueTarget.gameObject.SetActive(true);
            NotifyChallengeStarted("Level 2");
            StartCoroutine(SpawnEnemiesRoutine(30));
        }

        public override void StartLevelThree()
        {
            CurrentDifficulty = DifficultyLevel.Hard;
            statueTarget.gameObject.SetActive(true);
            NotifyChallengeStarted("Level 3");
            StartCoroutine(SpawnEnemiesRoutine(40));
        }

        private IEnumerator SpawnEnemiesRoutine(int totalEnemies)
        {
            challengeCompleted = false;
            challengeZone.SetActive(true);
            int spawnedEnemiesCount = 0;

            while (spawnedEnemiesCount < totalEnemies)
            {
                if (CurrentState == ChallengeState.Completed) yield break;

                int spawnCount = Mathf.Min(5, totalEnemies - spawnedEnemiesCount);
                for (int i = 0; i < spawnCount; i++)
                {
                    Vector3 randomPosition = GetRandomSpawnPosition();
                    GameObject enemyObj = Instantiate(spawnPrefab, randomPosition, Quaternion.identity);
                    Enemy enemy = enemyObj.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        if (i % 2 == 0)
                        {
                            enemy.ChangeTarget(statueTarget);
                        }
                        spawnedEnemies.Add(enemy);
                        TrackEnemyDeath(enemy);
                    }
                    spawnedEnemiesCount++;
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 position;
            do
            {
                float x = Random.Range(-fieldSize.x, fieldSize.x);
                float z = Random.Range(-fieldSize.y, fieldSize.y);
                position = new Vector3(transform.position.x - x, 0, transform.position.z - z);
            }
            while (position.magnitude < minDistanceFromCenter);

            return position;
        }

        private void TrackEnemyDeath(Enemy enemy)
        {
            StartCoroutine(WaitForEnemyDeath(enemy));
        }

        private IEnumerator WaitForEnemyDeath(Enemy enemy)
        {
            while (enemy.IsAlive())
            {
                yield return null;
            }

            spawnedEnemies.Remove(enemy);

            yield return new WaitForSeconds(5f);

            Destroy(enemy.gameObject);
        }
        private bool challengeCompleted = false; 

        private void CheckChallengeConditions()
        {
            if (IsStarted() && !challengeCompleted)
            {
                if (!player.IsAlive())
                {
                    CompleteChallenge(CompletedState.LOSE);
                }
                else if (statueHealth <= 0)
                {
                    CompleteChallenge(CompletedState.LOSE);
                }
                else if (spawnedEnemies.Count == 0)
                {
                    CompleteChallenge(CompletedState.WON);
                }
            }
        }

        private void CompleteChallenge(CompletedState completedState)
        {
            if (challengeCompleted) return;

            challengeCompleted = true; 

            Completed = completedState;
            CurrentState = ChallengeState.Completed;

            if (IsWon())
            {
                GetReward(player);
            }

            DisplayCompletionMessage();
            EndChallenge();
        }

        protected override void EndChallenge()
        {
            RemoveAllEnemies();
            challengeZone.SetActive(false);
            statueController.Heal(statueController.MaxHealth);
            statueTarget.gameObject.SetActive(false);
            Debug.Log($"Челлендж завершён. Результат: {Completed}");
            SwitchStart();
        }

        private void RemoveAllEnemies()
        {
            foreach (Enemy enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }

            spawnedEnemies.Clear();
            Debug.Log("Все враги из списка и с поля были удалены.");
        }
    }
}
