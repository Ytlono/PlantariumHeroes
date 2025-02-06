using UnityEngine;

namespace MyGameProject
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private float damage = 30f;
        [SerializeField]
        private Material weaponMaterial;
        [SerializeField]
        private Material weaponUltimateMaterial;
        private Renderer weaponRenderer;
        [SerializeField]
        private GameObject twist;
        private int currentLevel;

        public float Damage
        {
            get => damage;
            private set => damage = value;
        }

        public Material WeaponMaterial
        {
            get => weaponMaterial;
            set => weaponMaterial = value;
        }

        public Material WeaponUltimateMaterial
        {
            get => weaponUltimateMaterial;
            set => weaponUltimateMaterial = value;
        }

        public GameObject Twist
        {
            get => twist;
            set => twist = value;
        }

        public int CurrentWeaponLevel
        {
            get => currentLevel;
            set => currentLevel = value;
        }

        private void ChangeTexture(Texture texture)
        {
            WeaponMaterial.mainTexture = texture;
            WeaponMaterial.EnableKeyword("_EMISSION");
            WeaponMaterial.SetTexture("_EmissionMap", texture);
            WeaponMaterial.SetColor("_EmissionColor", Color.white * 2f);
        }
        public void ChangeWeaponSkin(Sprite skin)
        {
            if (WeaponMaterial == null)
            {
                return;
            }

            if (skin == null)
            {
                return;
            }

            ChangeTexture(skin.texture);
        }

        public void ActivateUltimate()
        {
            if (weaponRenderer != null)
            {
                weaponRenderer.material = weaponUltimateMaterial;
            }

            if (twist != null)
            {
                twist.SetActive(true);
            }
        }

        public void DeactivateUltimate()
        {
            if (weaponRenderer != null)
            {
                weaponRenderer.material = weaponMaterial;
            }

            if (twist != null)
            {
                Twist.SetActive(false);
            }
        }

        public void UpgradeDamage(float addDamage)
        {
            CurrentWeaponLevel++;
            Damage += addDamage;
        }

        private void Awake()
        {
            weaponRenderer = GetComponentInChildren<Renderer>();
        }

        public int GetCurrentLevel()
        {
            return CurrentWeaponLevel;
        }

        public void SaveWeaponData()
        {
            PlayerPrefs.SetInt("CurrentWeaponLevel", CurrentWeaponLevel);
            PlayerPrefs.SetString("WeaponMaterialTexture", WeaponMaterial.mainTexture.name);
            PlayerPrefs.Save();
        }

        public void LoadWeaponData()
        {
            string textureName = PlayerPrefs.GetString("WeaponMaterialTexture", "defaultTexture");
            Texture savedTexture = Resources.Load<Texture>("GamePrefabs/Skins/Weapon/WeaponSkins/" + textureName);
            CurrentWeaponLevel = PlayerPrefs.GetInt("CurrentWeaponLevel", 1);


            if (savedTexture != null && WeaponMaterial != null)
            {
                ChangeTexture(savedTexture);    
            }

        }
    }
}
