using Code.Scripts.Source.Audio;
using Code.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Source.UI
{
    [RequireComponent(typeof(Button), typeof(AudioSource))]
    public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Animation and feedback")]
        [SerializeField] private Sprite _originalSprite;
        [SerializeField] private Sprite _hoverSprite;
        [SerializeField] private Color _hoverColor = Color.white;
        [SerializeField] private float _animScaleDuration = 0.5f;

        private AudioSource _audio;
        private Button _button;
        private TMP_Text _title;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();

            CustomLogger.Assert(_audio, $"[UI Button {gameObject.name}] Fail to find AudioSource component", false);

            _button = GetComponent<Button>();
            _title = GetComponentInChildren<TMP_Text>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (AudioManager.Instance.gameObject.activeSelf && _audio)
                _audio.PlayOneShot(AudioManager.Instance.ClipsIndex.UIButtonSelected);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _button.image.sprite = _hoverSprite;
            _button.image.DOColor(_hoverColor, 0.5f);
            _button.transform.DOScale(1.1f, _animScaleDuration/2);

            if (_title)
                _title.DOColor(_hoverColor, 0.5f);

            if (AudioManager.Instance.gameObject.activeSelf && _audio)
                _audio.PlayOneShot(AudioManager.Instance.ClipsIndex.UIButtonHoverEnter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _button.image.sprite = _originalSprite;
            _button.image.DOColor(Color.white, 0.5f);
            _button.transform.DOScale(1f, _animScaleDuration/2);

            if (_title)
                _title.DOColor(Color.white, 0.5f);

            if (AudioManager.Instance.gameObject.activeSelf && _audio)
                _audio.PlayOneShot(AudioManager.Instance.ClipsIndex.UIButtonHoverExit);
        }

    }
}
