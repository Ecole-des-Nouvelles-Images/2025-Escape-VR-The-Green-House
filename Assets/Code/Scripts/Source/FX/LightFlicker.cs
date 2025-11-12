using UnityEngine;

namespace Code.Scripts.Source.FX
{
    [RequireComponent(typeof(Light))]
    public class LightFlicker : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private AnimationCurve _flickeringIntensity;
        [SerializeField] [Range(0, 1)] private float _timeScale = .5f;

        private Light _light;
        private float _baseIntensity;
        private float _clock;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _baseIntensity = _light.intensity;
        }

        private void Update()
        {
            _light.intensity =  _baseIntensity * _flickeringIntensity.Evaluate(_clock);
            _clock += Time.deltaTime * _timeScale;

            if (_clock > 1) _clock -= 1;
        }
    }
}
