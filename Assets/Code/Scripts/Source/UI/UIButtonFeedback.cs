using Code.Scripts.Source.Audio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Source.UI
{
    public class UIButtonFeedback : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _feedbackDuration = 2f;

        private AudioSource _audio;
        private Button _button;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            if (!_audio)
            {
                _audio = gameObject.AddComponent<AudioSource>();
                _audio.outputAudioMixerGroup = AudioManager.Instance.SFXMixerModule;
            }

            _button = GetComponent<Button>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (AudioManager.Instance.gameObject.activeSelf)
                _audio.PlayOneShot(AudioManager.Instance.ClipsIndex.UIButtonHoverEnter);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _button.transform.DOScale(1.1f, _feedbackDuration/2);

            if (AudioManager.Instance.gameObject.activeSelf)
                _audio.PlayOneShot(AudioManager.Instance.ClipsIndex.UIButtonSelected);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _button.transform.DOScale(1f, _feedbackDuration/2);

            if (AudioManager.Instance.gameObject.activeSelf)
                _audio.PlayOneShot(AudioManager.Instance.ClipsIndex.UIButtonHoverExit);
        }

    }
}
