using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Source.Narrator
{
    [RequireComponent(typeof(BoxCollider))]
    public class ProximityNarrator : MonoBehaviour
    {
        [SerializeField] private VoiceLineSO _proximityNarratorVoiceLine;

        private bool _isStopped;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MainCamera") && !_isStopped)
            {
                Narrator.Instance.PlayVoiceLine(_proximityNarratorVoiceLine);
                _isStopped = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _isStopped = false;
        }
    }
}
