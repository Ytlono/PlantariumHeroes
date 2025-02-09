using MyGameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeFirst : BaseChallenge
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private Vector2 fieldSize = new Vector2(15, 15);
    [SerializeField] private float minDistanceFromCenter = 8f;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private GameObject challengeZone;
    [SerializeField] private Text enemiesDefeatedText; 

    private List<Enemy> spawnedEnemies = new List<Enemy>();
    private Player player;
    private int totalEnemiesDefeated;

    public GameObject SpawnPrefab
    {
        get { return spawnPrefab; }
        set { spawnPrefab = value; }
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

    public Text EnemiesDefeatedText
    {
        get { return enemiesDefeatedText; }
        set { enemiesDefeatedText = value; }
    }

    private void Awake()
    {
        challengeZone.SetActive(false);
        Player = FindObjectOfType<Player>();
        if (EnemiesDefeatedText != null)
        {
            EnemiesDefeatedText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        CheckChallengeConditions();
    }

    public override void StartLevelOne()
    {
        CurrentDifficulty = DifficultyLevel.Easy;
        NotifyChallengeStarted("Level 1");
        StartCoroutine(SpawnEnemiesRoutine(20));
    }

    public override void StartLevelTwo()
    {
        CurrentDifficulty = DifficultyLevel.Medium;
        NotifyChallengeStarted("Level 2");
        StartCoroutine(SpawnEnemiesRoutine(30));
    }

    public override void StartLevelThree()
    {
        CurrentDifficulty = DifficultyLevel.Hard;
        NotifyChallengeStarted("Level 3");
        StartCoroutine(SpawnEnemiesRoutine(40));
    }

    private IEnumerator SpawnEnemiesRoutine(int totalEnemies)
    {
        challengeZone.SetActive(true);
        totalEnemiesDefeated = 0;
        UpdateEnemiesDefeatedText();
        if (EnemiesDefeatedText != null)
        {
            EnemiesDefeatedText.gameObject.SetActive(true);
        }

        int spawnedEnemiesCount = 0;

        while (spawnedEnemiesCount < totalEnemies)
        {
            if (CurrentState == ChallengeState.Completed) yield break;

            int spawnCount = Mathf.Min(5, totalEnemies - spawnedEnemiesCount);
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 randomPosition = GetRandomSpawnPosition();
                GameObject enemyObj = Instantiate(SpawnPrefab, randomPosition, Quaternion.identity);
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    SpawnedEnemies.Add(enemy);
                    TrackEnemyDeath(enemy);
                }
                spawnedEnemiesCount++;
            }
            yield return new WaitForSeconds(SpawnInterval);
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

        SpawnedEnemies.Remove(enemy);
        totalEnemiesDefeated++;
        UpdateEnemiesDefeatedText();

        yield return new WaitForSeconds(5f);

        Destroy(enemy.gameObject);
    }

    private void UpdateEnemiesDefeatedText()
    {
        if (EnemiesDefeatedText != null)
        {
            EnemiesDefeatedText.text = $"Enemies Defeated\n{totalEnemiesDefeated}/{spawnedEnemies.Count + totalEnemiesDefeated}";
        }
    }

    private void CheckChallengeConditions()
    {
        if (IsStarted())
        {
            if (Player != null && !Player.IsAlive())
            {
                CompleteChallenge(CompletedState.LOSE);
            }
            else if (SpawnedEnemies.Count == 0)
            {
                CompleteChallenge(CompletedState.WON);
            }
        }
    }

    private void CompleteChallenge(CompletedState completedState)
    {
        Completed = completedState;
        CurrentState = ChallengeState.Completed;

        if (EnemiesDefeatedText != null)
        {
            EnemiesDefeatedText.gameObject.SetActive(false);
        }

        if (IsWon())
        {
            GetReward(Player);
        }

        DisplayCompletionMessage();
        EndChallenge();
    }

    protected override void EndChallenge()
    {
        Debug.Log($"Челлендж завершён. Результат: {Completed}");
        RemoveAllEnemies();
        challengeZone.SetActive(false);
        SwitchStart();
    }

    private void RemoveAllEnemies()
    {
        foreach (Enemy enemy in SpawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        SpawnedEnemies.Clear();
    }
}