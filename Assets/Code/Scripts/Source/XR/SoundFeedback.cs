using Code.Scripts.Source.Audio;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Code.Scripts.Source.XR
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundFeedback : MonoBehaviour
    {
        [SerializeField] private float impactThreshold = 2f;
        [SerializeField] private float maxImpact = 10f;

        private AudioSource _audioSource;
        private float _impactForce;
        private XRGrabInteractable _xrGrabInteractable;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.SFXMixerModule;
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0.5f;
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
        }

        private void OnEnable()
        {
            _xrGrabInteractable.selectEntered.AddListener(GrabObject);
            _xrGrabInteractable.selectExited.AddListener(DropObject);
        }

        private void OnDisable()
        {
            _xrGrabInteractable.selectEntered.RemoveListener(GrabObject);
            _xrGrabInteractable.selectExited.RemoveListener(DropObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            _impactForce = collision.relativeVelocity.magnitude;
            if (_impactForce < impactThreshold) return;

            float normalized = Mathf.InverseLerp(impactThreshold, maxImpact, _impactForce);
            float volume = Mathf.Clamp01(normalized);

            _audioSource.PlayOneShot(AudioManager.Instance.ClipsIndex.Impact,volume);
        }


        private void GrabObject(SelectEnterEventArgs arg0)
        {
            _audioSource.PlayOneShot(AudioManager.Instance.ClipsIndex.ObjectGrab);
        }

        private void DropObject(SelectExitEventArgs arg0)
        {
            _audioSource.PlayOneShot(AudioManager.Instance.ClipsIndex.ObjectDrop);
        }
    }
}
