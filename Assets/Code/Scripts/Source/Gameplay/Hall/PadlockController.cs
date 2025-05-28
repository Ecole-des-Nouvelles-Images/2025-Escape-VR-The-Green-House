using System;
using Code.Scripts.Source.Audio;
using Code.Scripts.Source.GameFSM.States;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Narrator;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Source.Gameplay.Hall
{
    [RequireComponent(typeof(AudioSource))]
    public class PadlockController : MonoBehaviour
    {
        [Header("Padlock references")]
        [SerializeField] private Transform _padlockPrefab; 
        [SerializeField] private Transform _zoomTargetPoint;
        [SerializeField] private Vector3 _zoomedScale = new Vector3(2, 2, 2);
        [SerializeField] private float _zoomDuration = 0.5f;
        [SerializeField] private AnimationCurve _moveCurve;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private VoiceLineSO _narratorVoiceLine;
        
        [Header("Puzzle references")]
        [SerializeField] private GameObject _key;
        
        private Vector3 _originalScale;
        private Vector3 _originalPosition;
        private bool _isZoomed;
        private Animator _animator;
        private AudioSource _audioSource;

        private void OnEnable()
        {
            GameStateManager.Instance.GameStates.HallInProgress.OnCodeFound += UnlockPadLock;
            GameStateManager.Instance.GameStates.HallInProgress.OnRotated += PlayWheelSound;
        }

        private void OnDisable()
        {
            GameStateManager.Instance.GameStates.HallInProgress.OnCodeFound -= UnlockPadLock;
            GameStateManager.Instance.GameStates.HallInProgress.OnRotated -= PlayWheelSound;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            _originalScale = _padlockPrefab.localScale;
            _originalPosition = _padlockPrefab.position;
            
            _key.SetActive(false);
        }

        [ContextMenu("Zoom")]
        public void Zoom()
        {
            if (!_isZoomed)
            {
                _padlockPrefab.DOMove(_zoomTargetPoint.position, _zoomDuration).SetEase(_moveCurve);
                _padlockPrefab.DOScale(_zoomedScale, _zoomDuration).SetEase(_scaleCurve);
            }
            else
            {
                _padlockPrefab.DOMove(_originalPosition, _zoomDuration);
                _padlockPrefab.DOScale(_originalScale, _zoomDuration);
            }

            _audioSource.clip = AudioManager.Instance.ClipsIndex.PadlockZoom;
            _audioSource.Play();
            _isZoomed = !_isZoomed;
        }

        private void UnlockPadLock(GameBaseState gameBaseState, bool b, bool arg3)
        {
            _key.SetActive(true);
            _animator.SetTrigger("Open");
            Zoom();
        }

        private void PlayWheelSound(string s, int i)
        {
            _audioSource.clip = AudioManager.Instance.ClipsIndex.PadlockWheel;
            _audioSource.Play();
        }

        public void PlayFirstNarratorVoiceLine()
        {
            Narrator.Narrator.Instance.PlayVoiceLine(_narratorVoiceLine);
        }
    }
}