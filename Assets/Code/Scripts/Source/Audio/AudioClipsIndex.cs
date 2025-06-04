using System;
using System.Collections.Generic;
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
        [field:SerializeField] public AudioClip ObjectGrab { get; private set; }
        [field:SerializeField] public AudioClip Impact { get; private set; }

        [Header("Hall")]
        [field:SerializeField] public AudioClip PadlockWheel { get; private set; }
        [field:SerializeField] public AudioClip PadlockZoom { get; private set; }
        
        [Header("Lounge")]
        
        [Header("Drawers/Doors")]
        [field:SerializeField] public AudioClip OpenDrawer1 { get; private set; }
        [field:SerializeField] public AudioClip CloseDrawer1 { get; private set; }
        [field:SerializeField] public AudioClip OpenDrawer2 { get; private set; }
        [field:SerializeField] public AudioClip CloseDrawer2 { get; private set; }
        [field:SerializeField] public AudioClip OpenDrawer3 { get; private set; }
        [field:SerializeField] public AudioClip CloseDrawer3 { get; private set; }
        [field:SerializeField] public AudioClip OpenDrawer4 { get; private set; }
        [field:SerializeField] public AudioClip CloseDrawer4 { get; private set; }
        [field:SerializeField] public AudioClip OpenDoor1 { get; private set; }
        [field:SerializeField] public AudioClip CloseDoor1 { get; private set; }
        
        [Header("GreenHouse")]
        [field:SerializeField] public List<AudioClip> ShearsCut { get; private set; }
        [field:SerializeField] public AudioClip WaterCan { get; private set; }
        [field:SerializeField] public AudioClip GrownPlant { get; private set; }
        [field:SerializeField] public AudioClip Seed { get; private set; }
    }
}
