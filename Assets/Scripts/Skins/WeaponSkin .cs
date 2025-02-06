using System;
using UnityEngine;

namespace MyGameProject
{
    public class WeaponSkin : Skin
    {
        public string SkinSpritePath { get; private set; }

        public WeaponSkin(SkinData skinData)
        {
            Name = skinData.Name;
            Description = skinData.Description;
            RarityLevel = skinData.GetRarityLevel();
            Price = skinData.Price;
            DropChance = skinData.DropChance;
            Access = skinData.GetAccess();
            SkinSpritePath = skinData.SkinSprite;
        }

        public override void CountingPrice()
        {
            int basePrice = 50;
            Price = basePrice * (int)RarityLevel;
        }

        public override void ApplyEffect()
        {
            GameObject weapon = GameObject.FindWithTag("Weapon");

            if (weapon != null)
            {
                Renderer weaponRenderer = weapon.GetComponent<Renderer>();
                if (weaponRenderer != null && SkinSprite != null)
                {
                    weaponRenderer.material.mainTexture = SkinSprite.texture;
                }
            }
        }

        public override void DisplayInfo()
        {
            
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

                if (SkinSprite == null)
                {
                    
                }
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
}
