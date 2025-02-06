using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class PlayerSkin : Skin
    {
        public PlayerSkin(SkinData skinData)
        {
            Name = skinData.Name;
            Description = skinData.Description;
            RarityLevel = skinData.GetRarityLevel();
            Price = skinData.Price;
            DropChance = skinData.DropChance;
            Access = skinData.GetAccess();
            SkinSpritePath = skinData.SkinSprite;
        }


        public string SkinSpritePath { get; private set; }

        public override void CountingPrice()
        {
            int basePrice = 100;
            Price = basePrice * (int)RarityLevel;
        }

        public override void ApplyEffect()
        {
            Renderer playerRenderer = GameObject.FindWithTag("Player").GetComponent<Renderer>();
            if (playerRenderer != null && SkinSprite != null)
            {
                playerRenderer.material.mainTexture = SkinSprite.texture;
            }
        }

        public override void DisplayInfo()
        {
            Debug.Log($"Name: {Name}, Rarity: {RarityLevel}, Price: {Price}, Description: {Description}");
        }

        public override string ToString()
        {
            return $"Name: {Name}\nDescription: {Description}\nRarity: {RarityLevel}\nPrice: {Price}\nDrop Chance: {DropChance}\nAccess: {Access}\nSpritePath: {SkinSpritePath}";
        }

        public void LoadSprite()
        {
            if (!string.IsNullOrEmpty(SkinSpritePath))
            {
                SkinSprite = Resources.Load<Sprite>(SkinSpritePath);
            }
        }


        public override SkinData ToSkinData()
        {
            return new SkinData
            {
                Price = this.Price,
                Name = this.Name,
                Description = this.Description,
                RarityLevel = this.RarityLevel.ToString(), 
                DropChance = this.DropChance,
                Access = this.Access.ToString(),          
                SkinSprite = this.SkinSpritePath
            };
        }

    }

    [System.Serializable]
    public class SkinData
    {
        public int Price;
        public string Name;
        public string Description;
        public string RarityLevel; 
        public float DropChance;
        public string Access;    
        public string SkinSprite;

        public Rarity GetRarityLevel()
        {
            if (Enum.TryParse<Rarity>(RarityLevel, true, out var parsedRarity))
            {
                return parsedRarity;
            }
            return Rarity.COMMON; 
        }

        public Accesses GetAccess()
        {
            if (Enum.TryParse<Accesses>(Access, true, out var parsedAccess))
            {
                return parsedAccess;
            }
            return Accesses.LOCKED; 
        }
    }


    [Serializable]
    public class SkinDataList
    {
        public List<SkinData> skins = new List<SkinData>();
    }
}
