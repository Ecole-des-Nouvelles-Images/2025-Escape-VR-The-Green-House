using System;
using UnityEngine;

namespace Code.Scripts.Source.UI
{
    public class Credits : MonoBehaviour
    {
        [SerializeField] private float _scrollSpeed = 300f;
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _rectTransform.anchoredPosition += new Vector2(0, _scrollSpeed * Time.deltaTime);
        }
    }
}
