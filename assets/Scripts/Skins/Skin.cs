using System;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public enum Rarity
    {
        COMMON,         
        UNCOMMON,       
        RARE,           
        EPIC,           
        LEGENDARY,      
        MYTHICAL        
    }

    public enum Accesses
    {
        LOCKED,
        UNLOCKED,
    }

    public abstract class Skin
    {
        private int price;
        private string name;
        private string description;
        private Rarity rarity;
        private float dropChance; 
        public Accesses access;
        private Sprite skinSprite; 

        public Sprite SkinSprite
        {
            get { return skinSprite; }
            set { skinSprite = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Rarity RarityLevel
        {
            get { return rarity; }
            set { rarity = value; }
        }

        public float DropChance
        {
            get { return dropChance; }
            set { dropChance = value; }
        }

        public Accesses Access
        {
            get { return access; }
            set { access = value; }
        }


        public Skin() { }

        public abstract void CountingPrice();
        public abstract void ApplyEffect();
        public abstract void DisplayInfo();

        public abstract SkinData ToSkinData();
    }

}
