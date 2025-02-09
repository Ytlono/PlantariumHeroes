using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MyGameProject
{
    public class SkinManager : MonoBehaviour
    {
        private List<PlayerSkin> playerSkins;
        private List<WeaponSkin> weaponSkins;

        public List<PlayerSkin> PlayerSkinList
        {
            get { return playerSkins; }
            set { playerSkins = value; }
        }

        public List<WeaponSkin> WeaponSkinList
        {
            get { return weaponSkins; }
            set { weaponSkins = value; }
        }

        public Player Player { get; set; }
        public Weapon Weapon { get; set; }

        private void Awake()
        {
            Player = FindAnyObjectByType<Player>();
            Weapon = FindAnyObjectByType<Weapon>();
            PlayerSkinList = new List<PlayerSkin>();
            WeaponSkinList = new List<WeaponSkin>();

            LoadSkinsFromJson("PlayerDataSkins/PlayerSkinsData", typeof(PlayerSkin));
            LoadSkinsFromJson("WeaponDataSkins/WeaponSkinsData", typeof(WeaponSkin));

            if (playerSkins.Count > 0 || weaponSkins.Count > 0)
            {
                LoadSpritesForSkins();
            }
        }

        private void LoadSkinsFromJson(string fileName, Type skinType)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

            string json;
            if (File.Exists(fullPath))
            {
                json = File.ReadAllText(fullPath);
            }
            else
            {
                fullPath = Path.Combine(Application.streamingAssetsPath, $"{fileName}.json");
                if (File.Exists(fullPath))
                {
                    json = File.ReadAllText(fullPath);
                }
                else
                {
                    TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
                    if (jsonFile == null) return;
                    json = jsonFile.text;
                }
            }

            try
            {
                SkinDataList skinDataList = JsonUtility.FromJson<SkinDataList>(json);
                if (skinDataList == null || skinDataList.skins.Count == 0) return;

                foreach (var skinData in skinDataList.skins)
                {
                    if (skinType == typeof(PlayerSkin))
                    {
                        PlayerSkin skin = new PlayerSkin(skinData);
                        playerSkins.Add(skin);
                    }
                    else if (skinType == typeof(WeaponSkin))
                    {
                        WeaponSkin skin = new WeaponSkin(skinData);
                        weaponSkins.Add(skin);
                    }
                }
            }
            catch (Exception) { }
        }

        private void LoadSpritesForSkins()
        {
            foreach (var skin in playerSkins)
            {
                skin.LoadSprite();
            }

            foreach (var skin in weaponSkins)
            {
                skin.LoadSprite();
            }
        }

        public ReadOnlyCollection<PlayerSkin> GetPlayerSkins()
        {
            return playerSkins.AsReadOnly();
        }

        public ReadOnlyCollection<WeaponSkin> GetWeaponSkins()
        {
            return weaponSkins.AsReadOnly();
        }

        private void EnsureDirectoryExists(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private void UpdateSkinDataFile<T>(string filePath, List<T> skins) where T : Skin
        {
            var skinDataList = skins.Select(s => s.ToSkinData()).ToList();
            SaveSkinDataToFile(filePath, skinDataList);
        }

        private void SaveSkinDataToFile(string fileName, List<SkinData> skinDataList)
        {
            try
            {
                SkinDataList dataList = new SkinDataList { skins = skinDataList };
                string json = JsonUtility.ToJson(dataList, true);
                string fullPath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
                EnsureDirectoryExists(fullPath);

                File.WriteAllText(fullPath, json);
            }
            catch (Exception) { }
        }

        public void UnlockSkin(Skin skin)
        {
            if (skin == null) return;

            if (Player.MakePurchase(skin.Price))
            {
                skin.Access = Accesses.UNLOCKED;

                if (skin is PlayerSkin playerSkin)
                {
                    var originalSkin = PlayerSkinList.Find(s => s.Name == playerSkin.Name);
                    if (originalSkin != null)
                    {
                        originalSkin.Access = Accesses.UNLOCKED;
                        UpdateSkinDataFile("PlayerDataSkins/PlayerSkinsData", PlayerSkinList);
                    }
                }
                else if (skin is WeaponSkin weaponSkin)
                {
                    var originalSkin = WeaponSkinList.Find(s => s.Name == weaponSkin.Name);
                    if (originalSkin != null)
                    {
                        originalSkin.Access = Accesses.UNLOCKED;
                        UpdateSkinDataFile("WeaponDataSkins/WeaponSkinsData", WeaponSkinList);
                    }
                }
            }
        }

        public void ApplySkin(Skin skin)
        {
            if (skin is PlayerSkin playerSkin)
            {
                ApplyPlayerSkin(playerSkin.SkinSprite);
            }
            else if (skin is WeaponSkin weaponSkin)
            {
                ApplyWeaponSkin(weaponSkin.SkinSprite);
            }
        }

        public void ApplyPlayerSkin(Sprite skinSprite)
        {
            Player.ChangePlayerSkin(skinSprite);
        }

        public void ApplyWeaponSkin(Sprite skinSprite)
        {
            Weapon.ChangeWeaponSkin(skinSprite);
        }
    }
}
