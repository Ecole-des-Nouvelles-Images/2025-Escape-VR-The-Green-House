using UnityEngine;

namespace Code.Scripts.Source.Narrator
{
    [CreateAssetMenu(fileName = "new VoiceLine", menuName = "Custom SO/VoiceLine", order = 0)]
    public class VoiceLineSO : ScriptableObject
    {
        [field:SerializeField] public AudioClip Record { get; private set; }
        [field:SerializeField] [Multiline] public string Subtitle { get; private set; }
    }
}
