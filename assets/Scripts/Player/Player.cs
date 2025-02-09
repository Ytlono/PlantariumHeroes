using UnityEngine;

namespace MyGameProject
{
    public class Player : Character
    {
        [SerializeField]
        private float currentManna;
        private float maxManna = 100f;
        [SerializeField] private float mannaRechargeSpeed = 1f;

        [SerializeField]
        private int money;

        private Weapon weapon;
        public HealthBar healthBar;
        public ManaBar manaBar;

        [SerializeField]
        private PlayerMoney playerMoney;

        [SerializeField]
        private Material playerMaterial;

        private int healthLevel;
        private int mannaRechargeLevel;

        public float CurrentManna
        {
            set
            {
                if (value <= 0f)
                {
                    currentManna = 0f;
                }
                else if (value > maxManna)
                {
                    currentManna = maxManna;
                }
                else
                {
                    currentManna = value;
                }
            }
            get { return currentManna; }
        }

        public float MaxManna
        {
            get { return maxManna; }
            set { maxManna = value; }
        }

        public float MannaRechargeSpeed
        {
            get => mannaRechargeSpeed;
            set => mannaRechargeSpeed = value;
        }

        public Weapon Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }

        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public HealthBar HealthBar
        {
            get { return healthBar; }
            set { healthBar = value; }
        }

        public ManaBar ManaBar
        {
            get { return manaBar; }
            set { manaBar = value; }
        }

        public PlayerMoney PlayerMoney
        {
            get { return playerMoney; }
            set { playerMoney = value; }
        }

        public Material PlayerMaterial
        {
            get { return playerMaterial; }
            set { playerMaterial = value; }
        }

        public int HealthLevel
        {
            get { return healthLevel; }
            set { healthLevel = value; }
        }

        public int MannaRechargeLevel
        {
            get { return mannaRechargeLevel; }
            set { mannaRechargeLevel = value; }
        }

        private void Start()
        {
            weapon = GetComponentInChildren<Weapon>();
            healthBar = FindObjectOfType<HealthBar>();
            manaBar = FindObjectOfType<ManaBar>();
            playerMoney = FindObjectOfType<PlayerMoney>();

            if (CurrentManna == 0)
                CurrentManna = maxManna;
            if (Money == 0)
                Money = 6000;
            CurrentHealth = MaxHealth;
        }

        private void Update()
        {
            healthBar.UpdateHealthBar(CurrentHealth, MaxHealth);
            manaBar.UpdateManaBar(CurrentManna, MaxManna);
            playerMoney.UpdateBalance(Money);
            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(10f);
            }

            UpdateState();
        }

        public bool MakePurchase(int price)
        {
            if (price < 0)
            {
                Debug.Log("Цена не может быть отрицательной");
                return false;
            }

            if (Money < price)
            {
                Debug.Log("Недостаточно средств");
                return false;
            }

            Money -= price;
            Debug.Log("Покупка успешна!");
            return true;
        }

        public void IncreaseManna(float amount)
        {
            Debug.Log($"Increasing manna by {amount}");
            Debug.Log($"MannaRechargeSpeed {MannaRechargeSpeed}");
            CurrentManna += amount * MannaRechargeSpeed;
        }


        public bool IsManaFull()
        {
            return CurrentManna == 100;
        }

        public override void Heal(float healingAmount)
        {
            CurrentHealth += healingAmount;
        }

        public override void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            CheckHealth();
        }

        public override void UpdateState()
        {
            if (CurrentHealth == 0)
            {
                StateSetGet = State.DEAD;
            }
            else if (CurrentHealth > 0)
            {
                StateSetGet = State.INGAME;
            }
        }

        public void ChangePlayerSkin(Sprite skin)
        {
            PlayerMaterial.mainTexture = skin.texture;
        }

        public int GetCurrentHealthLevel()
        {
            return HealthLevel;
        }

        public int GetCurrentMannaRechargeLevel()
        {
            return MannaRechargeLevel;
        }

        public void UpgradePlayerHealth(float addHealth)
        {
            HealthLevel++;
            MaxHealth += addHealth;
        }

        public void UpgradeMannaRechargeSpeed(float addValue)
        {
            MannaRechargeLevel++;
            MannaRechargeSpeed += addValue;
        }

        private Coroutine emissionCoroutine;

        public void EnableEmission()
        {
            if (PlayerMaterial != null)
            {
                PlayerMaterial.EnableKeyword("_EMISSION");
                Debug.Log("Эмиссия включена.");
            }
        }

        public void DisableEmission()
        {
            if (PlayerMaterial != null)
            {
                PlayerMaterial.DisableKeyword("_EMISSION");
                Debug.Log("Эмиссия отключена.");
            }
        }


        public void SavePlayerData()
        {
            PlayerPrefs.SetInt("Money", Money);
            PlayerPrefs.SetFloat("MaxHealth", MaxHealth);
            PlayerPrefs.SetFloat("CurrentManna", CurrentManna);
            PlayerPrefs.SetFloat("CurrentHealth", CurrentHealth);
            PlayerPrefs.SetFloat("MannaRechargeSpeed", 1f);
            PlayerPrefs.SetString("PlayerMaterialTexture", PlayerMaterial.mainTexture.name);
            PlayerPrefs.SetInt("HealthLevel", HealthLevel);
            PlayerPrefs.SetInt("MannaRechargeLevel", MannaRechargeLevel);

            PlayerPrefs.Save();
        }

        public void LoadPlayerData()
        {
            Money = PlayerPrefs.GetInt("Money", 6000);
            MaxHealth = PlayerPrefs.GetFloat("MaxHealth", 1000f);
            CurrentHealth = PlayerPrefs.GetFloat("CurrentHealth", CurrentHealth);
            CurrentManna = PlayerPrefs.GetFloat("CurrentManna", MaxManna);
            MannaRechargeSpeed = PlayerPrefs.GetFloat("MannaRechargeSpeed", MannaRechargeSpeed);
            HealthLevel = PlayerPrefs.GetInt("HealthLevel", 1);
            MannaRechargeLevel = PlayerPrefs.GetInt("MannaRechargeLevel", 1);


            string textureName = PlayerPrefs.GetString("PlayerMaterialTexture", "defaultTexture");
            Texture savedTexture = Resources.Load<Texture>("GamePrefabs/Skins/Player/PlayerSkins/" + textureName);

            if (savedTexture != null && PlayerMaterial != null)
            {
                PlayerMaterial.mainTexture = savedTexture;
            }
        }
    }
}
