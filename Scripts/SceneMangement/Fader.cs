using UnityEngine;
using System.Collections;


namespace RPG.SceneMagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentFader;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float time)
        {
            if (currentFader != null)
            {
                StopCoroutine(currentFader);
            }

            currentFader = StartCoroutine(FadeOutCoroutine(time));
            yield return currentFader;

        }

        private IEnumerator FadeOutCoroutine(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public void FadeOutImmidately()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeIn(float time)
        {
            if (currentFader != null)
            {
                StopCoroutine(currentFader);
            }

            currentFader = StartCoroutine(FadeInCoroutine(time));
            return currentFader;

        }

        private IEnumerator FadeInCoroutine(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
