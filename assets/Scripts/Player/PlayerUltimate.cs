using System.Collections;
using UnityEngine;

namespace MyGameProject
{
    public class PlayerUltimate : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 2f;
        [SerializeField] private float speedMultiplier = 2f;
        [SerializeField] private float ultimateDuration = 10f;

        [SerializeField] private GameObject deathZone;

        private float originalSpeed;
        private float originalDamage;
        private bool isUltimateActive = false;

        private int currentBoostLevel;

        public float DamageMultiplier
        {
            get => damageMultiplier;
            set => damageMultiplier = value;
        }

        public float SpeedMultiplier
        {
            get => speedMultiplier;
            set => speedMultiplier = value;
        }

        public GameObject DeathZone
        {
            get => deathZone;
            set => deathZone = value;
        }

        public float UltimateDuration
        {
            get => ultimateDuration;
            set => ultimateDuration = value;
        }

        public bool IsUltimateActive
        {
            get => isUltimateActive;
            private set => isUltimateActive = value;
        }

        public int CurrentBoostLevel
        {
            get => currentBoostLevel;
            set => currentBoostLevel = value;
        }

        private float OriginalSpeed
        {
            get => originalSpeed;
            set => originalSpeed = value;
        }

        private float OriginalDamage
        {
            get => originalDamage;
            set => originalDamage = value;
        }

        private void Awake()
        {
            if (DeathZone == null)
            {
                Debug.LogError("DeathZone не назначен!");
            }
        }

        public void ActivateUltimate(Player player,PlayerRun playerRun)
        {
            if (IsUltimateActive)
            {
                Debug.LogWarning("Ульта уже активирована!");
                return;
            }

            StartCoroutine(UltimateRoutine(player,playerRun));
        }

        private IEnumerator UltimateRoutine(Player player, PlayerRun playerRun)
        {
            IsUltimateActive = true;

            OriginalDamage = player.Weapon.Damage;
            OriginalSpeed = playerRun.Speed;

            player.Weapon.UpgradeDamage(OriginalDamage * (DamageMultiplier - 1));
            playerRun.Speed *= SpeedMultiplier;
            DeathZone.SetActive(true);

            player.EnableEmission();
            player.Weapon.ActivateUltimate();

            yield return new WaitForSeconds(UltimateDuration);

            player.Weapon.UpgradeDamage(-OriginalDamage * (DamageMultiplier - 1));
            playerRun.Speed = OriginalSpeed;
            DeathZone.SetActive(false);

            player.DisableEmission();
            player.Weapon.DeactivateUltimate();
            IsUltimateActive = false;
        }

        public int GetCurrentBoostLevel()
        {
            return CurrentBoostLevel;
        }

        public void UpgradeUltimateMultipliers(float addValue)
        {
            CurrentBoostLevel++;
            DamageMultiplier += addValue;
            UltimateDuration += 1f;
        }

        public void SaveUltimateData()
        {
            PlayerPrefs.SetInt("CurrentBoostLevel", CurrentBoostLevel);
            PlayerPrefs.SetFloat("DamageMultiplier", DamageMultiplier);
            PlayerPrefs.SetFloat("UltimateDuration", UltimateDuration);
            PlayerPrefs.Save();
        }

        public void LoadUltimateData()
        {
            CurrentBoostLevel = PlayerPrefs.GetInt("CurrentBoostLevel", 1);
            DamageMultiplier = PlayerPrefs.GetFloat("DamageMultiplier", 2f);
            UltimateDuration = PlayerPrefs.GetFloat("UltimateDuration", 10f);
        }
    }
}
