using UnityEngine;

namespace MyGameProject
{
    public class PlayerData : MonoBehaviour
    {
        public Player Player { get; private set; }
        public PlayerUltimate PlayerUltimate { get; private set; }
        public Weapon Weapon { get; private set; }
        public UltimateZoneAttack UltimateZoneAttack { get; private set; }

        private void Awake()
        {
            Player = GetComponent<Player>();
            PlayerUltimate = GetComponent<PlayerUltimate>();
            Weapon = GetComponentInChildren<Weapon>();
            UltimateZoneAttack = GetComponentInChildren<UltimateZoneAttack>();
        }

        public void LoadAllPlayerData()
        {
            Player?.LoadPlayerData();
            Weapon?.LoadWeaponData();
            PlayerUltimate?.LoadUltimateData();
            UltimateZoneAttack?.LoadZoneData();
        }

        public void SaveAllPlayerData()
        {
            Player?.SavePlayerData();
            Weapon?.SaveWeaponData();
            PlayerUltimate?.SaveUltimateData();
            UltimateZoneAttack?.SaveZoneData();
        }
    }
}
