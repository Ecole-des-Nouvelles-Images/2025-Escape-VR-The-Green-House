using Code.Scripts.Source.Managers;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Source.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeScreen : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private float _fadeDuration;
        private AnimationCurve _fadeCurve;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _fadeDuration = SceneTransitionManager.FadeDuration;
            _fadeCurve = SceneTransitionManager.FadeCurve;

            // FadeOut();// Auto-start fade out on game launch
            _canvasGroup.alpha = 1; // Start opaque on game launch
        }

        public void FadeIn()
        {
            Fade(0,1, _fadeDuration);
        }

        public void FadeOut()
        {
            Fade(1,0, _fadeDuration);
        }

        private void Fade(float alphaIn, float alphaOut, float duration)
        {
            _canvasGroup.alpha = alphaIn;

            if (_fadeCurve == null)
                _canvasGroup.DOFade(alphaOut, duration);
            else
                _canvasGroup.DOFade(alphaOut, duration).SetEase(_fadeCurve);
        }
    }
}
