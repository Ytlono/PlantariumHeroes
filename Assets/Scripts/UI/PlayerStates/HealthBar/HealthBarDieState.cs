using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class HealthBarDieState : MonoBehaviour
    {
        private Player player;

        [SerializeField] private bool isEnableComponents;

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public bool IsEnableComponents
        {
            get { return isEnableComponents; }
            set { isEnableComponents = value; }
        }

        public void HideAndShowComponents(bool isEnable)
        {
            foreach (Transform child in transform)
            {
                Image image = child.GetComponent<Image>();
                if (image != null)
                {
                    image.enabled = isEnable;
                }

                Button button = child.GetComponent<Button>();
                if (button != null)
                {
                    button.enabled = isEnable;

                    Text buttonText = button.GetComponentInChildren<Text>();
                    if (buttonText != null)
                    {
                        buttonText.enabled = isEnable;
                    }
                }
            }
        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
            HideAndShowComponents(false);
        }

        private void Update()
        {
            switch (player.IsAlive())
            {
                case false:
                    HideAndShowComponents(true);
                    break;
                case true:
                    HideAndShowComponents(false);
                    break;
            }

            if (!player.IsAlive())
            {
                HideAndShowComponents(true);
            }
        }

    }
}
