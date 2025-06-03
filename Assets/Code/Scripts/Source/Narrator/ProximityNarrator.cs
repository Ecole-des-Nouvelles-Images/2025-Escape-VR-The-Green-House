using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Source.Narrator
{
    [RequireComponent(typeof(BoxCollider))]
    public class ProximityNarrator : MonoBehaviour
    {
        [SerializeField] private VoiceLineSO _proximityNarratorVoiceLine;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MainCamera"))
            {
                Narrator.Instance.PlayVoiceLine(_proximityNarratorVoiceLine);
            }
        }
    }
}
