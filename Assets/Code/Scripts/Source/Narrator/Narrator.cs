using Code.Scripts.Utils;
using UnityEngine;

namespace Code.Scripts.Source.Narrator
{
    [RequireComponent(typeof(AudioSource))]
    public class Narrator : MonoBehaviourSingleton<Narrator>
    {
        private AudioSource _audioSource;
        [SerializeField] private SubtitleModule subtitleModule;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayVoiceLine(VoiceLineSO voiceLine)
        {
            if (voiceLine == null) return;

            _audioSource.clip = voiceLine.Record;
            _audioSource.Play();
            
             subtitleModule.ShowSubtitle(voiceLine.Subtitle, voiceLine.Record.length + 5f);
             Debug.Log(voiceLine.Subtitle);
        }
    }
}