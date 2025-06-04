using Code.Scripts.Source.Managers;
using Code.Scripts.Source.XR;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Code.Scripts.Source.Narrator;


namespace Code.Scripts.Source.Gameplay.Lounge
{
    public class TVController : MonoBehaviour
    {
        [Header("Video References")] 
        //[SerializeField] private VideoClip _turnOnVideoClip;
        [SerializeField] private VideoClip _turnOffVideoClip;
        [SerializeField] private VideoClip _TvStaticVideoClip;
        [SerializeField] private VideoClip _VhsVideoClip;
    
        [Header("GameObject References")] 
        [SerializeField] private GameObject _tvScreen;
        [SerializeField] private XRBaseInteractable _remoteInteractable;
        [SerializeField] private Light _tvlight;
    
        [Header("Sound Effect")]
        [SerializeField] private AudioSource _tvTapeAudioSource;
        [SerializeField] private VoiceLineSO _tvOffVoiceLine;
        
        private VideoPlayer _videoPlayer;
        private Animator _animator;
        private XRSocketTagInteractor _socketTagInteractor;
        private Color _tvOnColor = Color.white;
        private Color _tvOffColor ;
        private bool _tvOn = false;
        private bool _cassetteInserted = false;
        private Material _tvScreenMaterial;

        private void OnEnable()
        {
           _remoteInteractable?.activated.AddListener(ToggleTvPower);
        }

        private void OnDisable()
        {
            _remoteInteractable?.activated.RemoveListener(ToggleTvPower);
        }

        private void Start()
        {
            _tvOffColor = _tvScreen.GetComponent<Renderer>().material.color;
            _tvScreenMaterial = _tvScreen.GetComponent<Renderer>().material;
            _animator = GetComponent<Animator>();
            _videoPlayer = GetComponentInChildren<VideoPlayer>();
            _socketTagInteractor = GetComponentInChildren<XRSocketTagInteractor>();
            _socketTagInteractor.selectEntered.AddListener(InsertVhs);
        }

        private void InsertVhs(SelectEnterEventArgs arg0)
        {
            _animator.SetTrigger("VhsOn");
            Destroy(_socketTagInteractor.firstInteractableSelected.transform.gameObject);
            _cassetteInserted = true;
            _tvTapeAudioSource.Play();
            Invoke("TurnOnTv",2f);
        }
        
        [ContextMenu("Turn On Tv")]
        private void TurnOnTv()
        {
            if (GameStateManager.Instance.GameStates.LoungePhase2._fusePlugged)
            {
                _tvlight.enabled = true;
                _tvOn = true;
                _videoPlayer.isLooping = true;
                PlayVideo(_cassetteInserted ? _VhsVideoClip : _TvStaticVideoClip);
                _tvScreenMaterial.color = _tvOnColor;
            }
            else
            {
                Narrator.Narrator.Instance.PlayVoiceLine(_tvOffVoiceLine);
            }
        }
    
        private void TurnOffTv()
        {
            _tvlight.enabled = false;
            _tvOn = false;
            _videoPlayer.isLooping = false;
            PlayVideo(_turnOffVideoClip);
            _tvScreenMaterial.color = _tvOffColor;
        }


      
        private void ToggleTvPower(ActivateEventArgs args)
        {
            _tvOn = !_tvOn;
            if (_tvOn)
            { 
                TurnOnTv();
            }
            else
            { 
                TurnOffTv();
            }
        }

        private void PlayVideo(VideoClip clip)
        {
            _videoPlayer.clip = clip;
            _videoPlayer.Play();
        }
    
    }
}
