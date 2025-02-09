using UnityEngine;

namespace MyGameProject
{
    public class UltimateZoneAttack : MonoBehaviour
    {
        [SerializeField]
        private float ultimateDamage = 20; 
        [SerializeField]
        private float damageInterval = 1f;
        private int currentZoneLevel;

        public int CurrentZoneLevel
        {
            get { return currentZoneLevel; }
            set { currentZoneLevel = value; }
        }

        public float UltimateDamage {
            get { return ultimateDamage;}
            set { ultimateDamage = value; }
        }

        public float DamageInterval => damageInterval; 

        public int GetCurrentZoneLevel()
        {
            return CurrentZoneLevel;
        }

        public void UpgradeUltimateZoneDamage(float addDamage)
        {
            CurrentZoneLevel++;
            UltimateDamage += addDamage;
        }

        public void SaveZoneData()
        {
            PlayerPrefs.SetInt("CurrentZoneLevel", CurrentZoneLevel);
            PlayerPrefs.SetFloat("UltimateDamage", UltimateDamage);

            PlayerPrefs.Save();
        }

        public void LoadZoneData()
        {
            CurrentZoneLevel = PlayerPrefs.GetInt("CurrentZoneLevel", 1);
            UltimateDamage = PlayerPrefs.GetFloat("UltimateDamage", 1);
        }
    }
}
