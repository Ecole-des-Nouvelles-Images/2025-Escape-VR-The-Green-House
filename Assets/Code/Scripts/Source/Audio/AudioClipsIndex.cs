using System;
using UnityEngine;

namespace Code.Scripts.Source.Audio
{
    [Serializable]
    public class AudioClipsIndex
    {
        // TODO: All audio clips of the project
        [Header("UI")]
        [field:SerializeField] public AudioClip UIButtonSelected { get; private set; }
        [field:SerializeField] public AudioClip UIButtonHoverEnter { get; private set; }
        [field:SerializeField] public AudioClip UIButtonHoverExit { get; private set; }

        [Header("Ambiance")]
        [field:SerializeField] public AudioClip RainOutDoor { get; private set; }
        [field:SerializeField] public AudioClip RainInDoor { get; private set; }
        [field:SerializeField] public AudioClip MenuMusic { get; private set; }
        [field:SerializeField] public AudioClip GameMusic { get; private set; }
        
        [Header("Generic")]
        [field:SerializeField] public AudioClip OpenDoor { get; private set; }
        [field:SerializeField] public AudioClip InsertKey { get; private set; }
        [field:SerializeField] public AudioClip ObjectDrop { get; private set; }

        [Header("Hall")]
        [field:SerializeField] public AudioClip PadlockWheel { get; private set; }
        [field:SerializeField] public AudioClip PadlockZoom { get; private set; }
    }
}
