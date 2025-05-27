using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace Code.Scripts.Source.Narrator
{
    public class SubtitleModule : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private float fadeDuration = 0.5f;
        [SerializeField] private float typewriterSpeed = 0.02f;

        private Coroutine subtitleRoutine;

        public void ShowSubtitle(string subtitle, float displayDuration)
        {
            if (subtitleRoutine != null)
                StopCoroutine(subtitleRoutine);

            subtitleRoutine = StartCoroutine(HandleSubtitle(subtitle, displayDuration));
        }

        private IEnumerator HandleSubtitle(string text, float displayDuration)
        {
            subtitleText.text = "";
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(true);

            canvasGroup.DOFade(1, fadeDuration);

            foreach (char c in text)
            {
                subtitleText.text += c;
                yield return new WaitForSeconds(typewriterSpeed);
            }

            yield return new WaitForSeconds(displayDuration);
            canvasGroup.DOFade(0, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);

            canvasGroup.gameObject.SetActive(false);
            subtitleRoutine = null;
        }
    }
}