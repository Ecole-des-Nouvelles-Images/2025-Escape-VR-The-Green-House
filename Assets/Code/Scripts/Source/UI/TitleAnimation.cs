using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Source.UI
{
    public class TitleAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _titleTransform;

        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 1f;
        //[SerializeField] private float _scaleInDuration = 1.2f;
        [SerializeField] private float _delay = 0.3f;

        [Header("Wobble Settings")]
        [SerializeField] private float _wobbleScale = 0.05f;
        [SerializeField] private float _wobbleDuration = 1.5f;

        private void Start()
        {

            _canvasGroup.alpha = 0f;
            //  _titleTransform.localScale = Vector3.zero;

            AnimateTitle();
        }

        private void AnimateTitle()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(_canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.OutBack).SetDelay(_delay));
            //   sequence.Join(_titleTransform.DOScale(Vector3.one, _scaleInDuration).SetEase(Ease.OutBack).SetDelay(_delay));

            sequence.OnComplete(StartWobble);
        }

        private void StartWobble()
        {
            _titleTransform.DOScaleX(1f + _wobbleScale, _wobbleDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            _titleTransform.DOScaleY(1f - _wobbleScale, _wobbleDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}
