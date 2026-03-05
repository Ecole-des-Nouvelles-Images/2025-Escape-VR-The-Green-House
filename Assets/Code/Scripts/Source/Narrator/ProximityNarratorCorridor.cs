using System;
using Code.Scripts.Source.Managers;
using UnityEngine;

namespace Code.Scripts.Source.Narrator
{
    [RequireComponent(typeof(BoxCollider))]
    public class ProximityNarratorCorridor : MonoBehaviour
    {
        [SerializeField] private VoiceLineSO _proximityNarratorVoiceLine;
        [SerializeField] private bool _hasToBePlayedOnce;

        private bool _hasBeenPlayed;
        private bool _isStopped;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("MainCamera") || _isStopped || !GameStateManager.Instance.GreenHouseIsLocked) return;

            if (_hasToBePlayedOnce)
            {
                if (_hasBeenPlayed) return;

                Narrator.Instance.PlayVoiceLine(_proximityNarratorVoiceLine);
                _isStopped = true;
                _hasBeenPlayed = true;
            }
            else
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
