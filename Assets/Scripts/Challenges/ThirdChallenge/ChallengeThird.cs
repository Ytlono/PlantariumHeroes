using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class ChallengeThird : BaseChallenge 
    {
        [SerializeField] private GameObject spherePrefab;
        [SerializeField] private Vector2 fieldSize = new Vector2(15, 15);
        [SerializeField] private float minDistanceFromCenter = 5f;
        [SerializeField] private float challengeTime = 40f;
        [SerializeField] private Text timeText;
        [SerializeField] private Text spheresCollectedText;

        private Player player;
        private float remainingTime;
        private List<GameObject> spawnedSpheres = new List<GameObject>();
        private int spheresCollected;
        private bool challengeCompleted = false;

        public GameObject SpherePrefab
        {
            get => spherePrefab;
            private set => spherePrefab = value;
        }

        public Vector2 FieldSize
        {
            get => fieldSize;
            private set => fieldSize = value;
        }

        public float MinDistanceFromCenter
        {
            get => minDistanceFromCenter;
            private set => minDistanceFromCenter = value;
        }

        public float ChallengeTime
        {
            get => challengeTime;
            private set => challengeTime = value;
        }

        public Text TimeText
        {
            get => timeText;
            private set => timeText = value;
        }

        public Text SpheresCollectedText
        {
            get => spheresCollectedText;
            private set => spheresCollectedText = value;
        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
            remainingTime = ChallengeTime;

            if (TimeText != null) TimeText.gameObject.SetActive(false);
            if (SpheresCollectedText != null) SpheresCollectedText.gameObject.SetActive(false);
            if (CompletionMessage != null) CompletionMessage.gameObject.SetActive(false);
        }


        public override void StartLevelOne()
        {
            CurrentDifficulty = DifficultyLevel.Easy;
            NotifyChallengeStarted("Level 1");
            StartChallenge(15);
        }

        public override void StartLevelTwo()
        {
            CurrentDifficulty = DifficultyLevel.Medium;
            NotifyChallengeStarted("Level 2");
            StartChallenge(25);
        }

        public override void StartLevelThree()
        {
            CurrentDifficulty = DifficultyLevel.Hard;
            NotifyChallengeStarted("Level 3");
            StartChallenge(35);
        }

        private void StartChallenge(int totalSpheres)
        {
            remainingTime = ChallengeTime;
            spheresCollected = 0;

            if (TimeText != null) TimeText.gameObject.SetActive(true);
            if (SpheresCollectedText != null) SpheresCollectedText.gameObject.SetActive(true);

            UpdateUI();
            SpawnSpheres(totalSpheres);
        }


        private void Update()
        {
            if (IsStarted() && !challengeCompleted)
            {
                UpdateChallengeTime();
                CheckChallengeConditions();
            }
        }

        private void UpdateChallengeTime()
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                CompleteChallenge(CompletedState.LOSE);
            }
            UpdateUI();
        }

        private void SpawnSpheres(int totalSpheres)
        {
            for (int i = 0; i < totalSpheres; i++)
            {
                Vector3 randomPosition = GetRandomSpawnPosition();
                GameObject sphere = Instantiate(SpherePrefab, randomPosition, Quaternion.identity);
                spawnedSpheres.Add(sphere);
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 position;
            do
            {
                float x = Random.Range(-FieldSize.x, FieldSize.x);
                float z = Random.Range(-FieldSize.y, FieldSize.y);
                position = new Vector3(transform.position.x + x, 0, transform.position.z + z);
            }
            while (position.magnitude < MinDistanceFromCenter);

            return position;
        }

        private void CheckChallengeConditions()
        {
            if (spawnedSpheres.Count == 0)
            {
                CompleteChallenge(CompletedState.WON);
            }
        }

        public void RemoveSphere(GameObject sphere)
        {
            if (spawnedSpheres.Contains(sphere))
            {
                spawnedSpheres.Remove(sphere);
                spheresCollected++;
                UpdateUI();
            }

            if (spawnedSpheres.Count == 0)
            {
                CompleteChallenge(CompletedState.WON);
            }
        }

        private void UpdateUI()
        {
            if (TimeText != null)
            {
                TimeText.text = $"Time: {Mathf.CeilToInt(remainingTime)}s";
            }

            if (SpheresCollectedText != null)
            {
                SpheresCollectedText.text = $"S:{spheresCollected}/{spawnedSpheres.Count + spheresCollected}";
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
            if (TimeText != null) TimeText.gameObject.SetActive(false);
            if (SpheresCollectedText != null) SpheresCollectedText.gameObject.SetActive(false);

            RemoveAllSpheres();
            SwitchStart();
        }


        private void RemoveAllSpheres()
        {
            foreach (GameObject sphere in spawnedSpheres)
            {
                if (sphere != null)
                {
                    Destroy(sphere);
                }
            }

            spawnedSpheres.Clear();
        }

    }
}
