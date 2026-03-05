using System.Collections.Generic;
using Code.Scripts.Source.Audio;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

namespace Code.Scripts.Source.Gameplay.GreenHouse
{
    [RequireComponent(typeof(AudioSource))]
    public class shears : MonoBehaviour
    {
    
        private XRBaseInteractable interactable; 
        private AudioSource _audioSource;
        private List<AudioClip> _shearsClips; 
        private Animator _animator;
    
        private void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.SFXMixerModule;
            _audioSource.playOnAwake = false;
            _shearsClips = AudioManager.Instance.ClipsIndex.ShearsCut;
      
        }
      
        void OnEnable()
        {
            interactable.activated.AddListener(CutPlant);
        }

        void OnDisable() 
        { 
            interactable.activated.RemoveListener(CutPlant);
        }

     
        private void CutPlant(ActivateEventArgs arg0)
        {
            _animator.SetTrigger("Cut");
            _audioSource.clip = _shearsClips[Random.Range(0, _shearsClips.Count)];
            _audioSource.Play();
        }
    }
}
