using System;
using Code.Scripts.Source.GameFSM.States;
using Code.Scripts.Source.Managers;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Source.Gameplay.Hall
{
    public class PadlockController : MonoBehaviour
    {
        [SerializeField] private Transform _padlockPrefab; 
        [SerializeField] private Transform _zoomTargetPoint;
        [SerializeField] private Vector3 _zoomedScale = new Vector3(2, 2, 2);
        [SerializeField] private float _zoomDuration = 0.5f;
        [SerializeField] private AnimationCurve _moveCurve;
        [SerializeField] private AnimationCurve _scaleCurve;


        private Vector3 _originalScale;
        private Vector3 _originalPosition;
        private bool _isZoomed;
        private Animator _animator;
        

        private void OnEnable()
        {
            GameStateManager.Instance.GameStates.HallInProgress.OnCodeFound += UnlockPadLock;
        }

        private void OnDisable()
        {
            GameStateManager.Instance.GameStates.HallInProgress.OnCodeFound -= UnlockPadLock;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _originalScale = _padlockPrefab.localScale;
            _originalPosition = _padlockPrefab.position;
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

            _isZoomed = !_isZoomed;
        }

        private void UnlockPadLock(GameBaseState gameBaseState, bool b, bool arg3)
        {
            _animator.SetTrigger("Open");
            Zoom();
        }
    }
}