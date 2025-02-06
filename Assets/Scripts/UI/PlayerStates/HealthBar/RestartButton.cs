using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class RestartButton : MonoBehaviour
    {
        [SerializeField]
        private float beatDuration = 0.2f;
        [SerializeField]
        private float minScale = 0.9f;
        [SerializeField]
        private float maxScale = 1f;

        private Button button;
        private Player player;
        private PlayerDie playerDie;

        public float BeatDuration
        {
            get { return beatDuration; }
            set { beatDuration = value; }
        }

        public float MinScale
        {
            get { return minScale; }
            set { minScale = value; }
        }

        public float MaxScale
        {
            get { return maxScale; }
            set { maxScale = value; }
        }

        public Button Button
        {
            get { return button; }
            set { button = value; }
        }

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public PlayerDie PlayerDie
        {
            get { return playerDie; }
            set { playerDie = value; }
        }

        void Start()
        {
            button = GetComponent<Button>();
            player = FindAnyObjectByType<Player>();
            playerDie = FindAnyObjectByType<PlayerDie>();
            StartCoroutine(ButtonAnimation());
        }

        private IEnumerator ButtonAnimation()
        {
            while (true)
            {
                yield return StartCoroutine(ScaleHeart(maxScale));
                yield return StartCoroutine(ScaleHeart(minScale));
            }
        }

        private IEnumerator ScaleHeart(float targetScale)
        {
            float currentTime = 0f;
            Vector3 initialScale = button.transform.localScale;
            Vector3 targetScaleVector = new Vector3(targetScale, targetScale, 1);

            while (currentTime < beatDuration)
            {
                currentTime += Time.deltaTime;
                button.transform.localScale = Vector3.Lerp(initialScale, targetScaleVector, currentTime / beatDuration);
                yield return null;
            }

            button.transform.localScale = targetScaleVector;
        }

        public void OnButtonClick()
        {
            player.Heal(100f);
            StartCoroutine(ActivateRecoveryWithDelay());
        }

        private IEnumerator ActivateRecoveryWithDelay()
        {
            yield return new WaitForSeconds(0.1f);
            playerDie.ActivateDieRecoveryAnimation();
        }
    }
}
