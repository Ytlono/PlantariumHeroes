using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MyGameProject.ShopPanel;

namespace MyGameProject
{
    public class SkinShop : MonoBehaviour
    {
        public CurrentSkinDataInShop CurrentSkinForView { get; set; }

        [SerializeField]
        private Text currentSkinName;
        [SerializeField]
        private Text currentSkinDescription;
        [SerializeField]
        private Image currentSkinImage;
        [SerializeField]
        private Image currentSkinImageBorder;
        [SerializeField]
        private Text currentSkinPrice;
        [SerializeField]
        private Text currentSkinRarity;
        [SerializeField]
        private Text buyButtonText;

        private SkinManager skinManager;

        private List<Skin> currentSkinList;
        private IReadOnlyList<PlayerSkin> playerSkinList;
        private IReadOnlyList<WeaponSkin> weaponSkinList;

        private bool showingPlayerSkins = true;

        public Text CurrentSkinName
        {
            get { return currentSkinName; }
            set { currentSkinName = value; }
        }

        public Text CurrentSkinDescription
        {
            get { return currentSkinDescription; }
            set { currentSkinDescription = value; }
        }

        public Image CurrentSkinImage
        {
            get { return currentSkinImage; }
            set { currentSkinImage = value; }
        }

        public Image CurrentSkinImageBorder
        {
            get { return currentSkinImageBorder; }
            set { currentSkinImageBorder = value; }
        }

        public Text CurrentSkinPrice
        {
            get { return currentSkinPrice; }
            set { currentSkinPrice = value; }
        }

        public Text CurrentSkinRarity
        {
            get { return currentSkinRarity; }
            set { currentSkinRarity = value; }
        }

        public Text BuyButtonText
        {
            get { return buyButtonText; }
            set { buyButtonText = value; }
        }

        public SkinManager SkinManager
        {
            get { return skinManager; }
            set { skinManager = value; }
        }

        public List<Skin> CurrentSkinList
        {
            get { return currentSkinList; }
            set { currentSkinList = value; }
        }

        public IReadOnlyList<PlayerSkin> PlayerSkinList
        {
            get { return playerSkinList; }
            set { playerSkinList = value; }
        }

        public IReadOnlyList<WeaponSkin> WeaponSkinList
        {
            get { return weaponSkinList; }
            set { weaponSkinList = value; }
        }

        public bool ShowingPlayerSkins
        {
            get { return showingPlayerSkins; }
            set { showingPlayerSkins = value; }
        }

        private void Start()
        {
            InitializeSkinManager();
            LoadSkins();
            SwitchToPlayerSkins();
        }

        private void InitializeSkinManager()
        {
            SkinManager = FindObjectOfType<SkinManager>();

            if (SkinManager == null)
            {
                return;
            }
        }

        private void LoadSkins()
        {
            PlayerSkinList = SkinManager.GetPlayerSkins();
            WeaponSkinList = SkinManager.GetWeaponSkins();

            if (PlayerSkinList.Count == 0 && WeaponSkinList.Count == 0)
            {
                return;
            }
        }

        public void SetShopCurrentSkin(int index)
        {
            if (index < 0 || index >= CurrentSkinList.Count)
            {
                return;
            }

            var selectedSkin = CurrentSkinList[index];
            UpdateSkinUI(selectedSkin);
        }

        private void UpdateSkinUI(Skin skin)
        {
            CurrentSkinForView = new CurrentSkinDataInShop(CurrentSkinList.IndexOf(skin), skin);

            CurrentSkinName.text = skin.Name;
            CurrentSkinDescription.text = skin.Description;
            CurrentSkinImage.sprite = skin.SkinSprite;
            CurrentSkinPrice.text = skin.Price.ToString();
            CurrentSkinRarity.text = skin.RarityLevel.ToString();

            CurrentSkinImageBorder.color = GetRarityColor(skin.RarityLevel);
            BuyButtonText.text = skin.Access == Accesses.UNLOCKED ? "Apply" : "Buy";
        }

        private Color GetRarityColor(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.COMMON => new Color(144f / 255f, 255f / 255f, 255f / 255f),
                Rarity.UNCOMMON => new Color(144f / 255f, 255f / 255f, 165f / 255f),
                Rarity.RARE => new Color(171f / 255f, 111f / 255f, 255f / 255f),
                Rarity.EPIC => new Color(242f / 255f, 255f / 255f, 111f / 255f),
                Rarity.LEGENDARY => new Color(242f / 255f, 132f / 255f, 89f / 255f),
                Rarity.MYTHICAL => Color.red,
                _ => Color.white,
            };
        }

        public void OnClickButtonNext()
        {
            CurrentSkinForView.Index = (CurrentSkinForView.Index + 1) % CurrentSkinList.Count;
            SetShopCurrentSkin(CurrentSkinForView.Index);
        }

        public void OnClickButtonPrevious()
        {
            CurrentSkinForView.Index = (CurrentSkinForView.Index - 1 + CurrentSkinList.Count) % CurrentSkinList.Count;
            SetShopCurrentSkin(CurrentSkinForView.Index);
        }

        public void OnClickButtonBuy()
        {
            var currentSkin = CurrentSkinForView.Skin;

            if (currentSkin.Access == Accesses.UNLOCKED)
            {
                SkinManager.ApplySkin(currentSkin);
            }
            else
            {
                SkinManager.UnlockSkin(currentSkin);
                BuyButtonText.text = "Apply";
            }
        }

        public void OnClickPlayerSkin()
        {
            SwitchToPlayerSkins();
        }

        public void OnClickWeaponSkin()
        {
            SwitchToWeaponSkins();
        }

        private void SwitchToPlayerSkins()
        {
            ShowingPlayerSkins = true;
            CurrentSkinList = new List<Skin>(PlayerSkinList);
            CurrentSkinForView = new CurrentSkinDataInShop(0, CurrentSkinList[0]);
            SetShopCurrentSkin(0);
        }

        private void SwitchToWeaponSkins()
        {
            ShowingPlayerSkins = false;
            CurrentSkinList = new List<Skin>(WeaponSkinList);
            CurrentSkinForView = new CurrentSkinDataInShop(0, CurrentSkinList[0]);
            SetShopCurrentSkin(0);
        }

        public class CurrentSkinDataInShop
        {
            public int Index { get; set; }
            public Skin Skin { get; set; }

            public CurrentSkinDataInShop(int index, Skin skin)
            {
                Index = index;
                Skin = skin;
            }
        }
    }
}
