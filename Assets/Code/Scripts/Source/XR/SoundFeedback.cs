using Code.Scripts.Source.Audio;
using UnityEngine;

namespace Code.Scripts.Source.XR
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundFeedback : MonoBehaviour
    {
        [SerializeField] private float impactThreshold = 2f;
        [SerializeField] private float maxImpact = 10f;

        private AudioSource _audioSource;
        private float _impactForce;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.SFXMixerModule;
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0.5f;
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            _impactForce = collision.relativeVelocity.magnitude;
            if (_impactForce < impactThreshold) return;
        
            float normalized = Mathf.InverseLerp(impactThreshold, maxImpact, _impactForce);
            float volume = Mathf.Clamp01(normalized);

            _audioSource.PlayOneShot(AudioManager.Instance.ClipsIndex.ObjectDrop,volume);
        }
    }
}
