using System;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;
using UnityEngine;

namespace Code.Scripts.Source.UI
{
    public class Credits : MonoBehaviour
    {
        [SerializeField] private SceneType _destination;
        [SerializeField] private float _scrollSpeed = 300f;
        private RectTransform _rectTransform;
        private bool _isSwitching;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _isSwitching = false;
        }

        private void Update()
        {
            _rectTransform.anchoredPosition += new Vector2(0, _scrollSpeed * Time.deltaTime);

            if (_rectTransform.anchoredPosition.y >= 3500f && !_isSwitching)
            {
                SceneLoader.Instance.SwitchScene(_destination);
                _isSwitching = true;
            }
        }
        
        
    }
}
