using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class GameManager : MonoBehaviour
    {
        private PlayerData playerData;
        private List<Enemy> enemies;

        private void Start()
        {
            enemies = new List<Enemy>();

            if (playerData == null)
            {
                playerData = FindObjectOfType<PlayerData>();
            }

            if (enemies.Count == 0)
            {
                enemies.AddRange(FindObjectsOfType<Enemy>());
            }

            LoadGameData();
            HandlePlayerState();
        }

        private void Update()
        {
            HandlePlayerState();
        }

        private void HandlePlayerState()
        {
            if (playerData == null || !playerData.Player.IsAlive())
            {
                StopEnemiesActions();
            }
            else
            {
                ResumeEnemiesActions();
            }
        }

        private void StopEnemiesActions()
        {
            foreach (var enemy in enemies)
            {
                if (enemy != null && enemy.IsAlive())
                {
                    EnemyController controller = enemy.GetComponent<EnemyController>();
                    if (controller != null)
                    {
                        controller.StopActions();
                    }
                }
            }
        }

        private void ResumeEnemiesActions()
        {
            foreach (var enemy in enemies)
            {
                if (enemy != null && enemy.IsAlive())
                {
                    EnemyController controller = enemy.GetComponent<EnemyController>();
                    if (controller != null)
                    {
                        controller.ResumeActions();
                    }
                }
            }
        }

        private void LoadGameData()
        {
            if (playerData != null)
            {
                playerData.LoadAllPlayerData();
            }
        }

        private void SaveGameData()
        {
            if (playerData != null)
            {
                playerData.SaveAllPlayerData();
            }
        }

        public void OnApplicationQuit()
        {
            SaveGameData();
        }
    }
}
