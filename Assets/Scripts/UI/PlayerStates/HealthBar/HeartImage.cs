using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameProject
{
    public class HeartImage : MonoBehaviour
    {
        [SerializeField]
        private Image heart;

        [SerializeField]
        private float beatDuration = 0.2f;

        [SerializeField]
        private float minScale = 0.8f;

        [SerializeField]
        private float maxScale = 1.2f;

        public Image Heart
        {
            get { return heart; }
            set { heart = value; }
        }

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

        private void Start()
        {
            StartCoroutine(BeatHeart());
        }

        private IEnumerator BeatHeart()
        {
            while (true)
            {
                yield return StartCoroutine(ScaleHeart(MaxScale));
                yield return StartCoroutine(ScaleHeart(MinScale));
            }
        }

        private IEnumerator ScaleHeart(float targetScale)
        {
            float currentTime = 0f;
            Vector3 initialScale = heart.transform.localScale;
            Vector3 targetScaleVector = new Vector3(targetScale, targetScale, 1);

            while (currentTime < beatDuration)
            {
                currentTime += Time.deltaTime;
                heart.transform.localScale = Vector3.Lerp(initialScale, targetScaleVector, currentTime / beatDuration);
                yield return null;
            }

            heart.transform.localScale = targetScaleVector;
        }
    }
}
