using Code.Scripts.Source.Audio;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;
using Code.Scripts.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Source.UI
{
    public class PauseMenuController: MonoBehaviour
    {
        [SerializeField] private RectTransform _root;
        [SerializeField] private RectTransform _mainPanel;
        [SerializeField] private RectTransform _pausePanel;
        [Space]
        [SerializeField] private float _slideAnimationDuration = 1.5f;
        [SerializeField] private Ease _slideAnimationEasing;

        [Header("Main panel")]
        [SerializeField] private Button _mainButtonResume;
        [SerializeField] private Button _mainButtonOptions;
        [SerializeField] private Button _mainButtonMainMenu;
        [SerializeField] private Button _mainButtonExit;

        [Header("Options panel")]
        [SerializeField] private Button _optionsButtonToggleMaster;
        [SerializeField] private Button _optionsButtonToggleAmbient;
        [SerializeField] private Button _optionsButtonToggleSFX;
        [SerializeField] private Button _optionsButtonReturn;
        [Space]
        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _ambientVolume;
        [SerializeField] private Slider _sfxVolume;
        [Space]
        [SerializeField] private Sprite _masterVolumeIcon;
        [SerializeField] private Sprite _ambientVolumeIcon;
        [SerializeField] private Sprite _sfxVolumeIcon;
        [SerializeField] private Sprite _masterVolumeIconMuted;
        [SerializeField] private Sprite _ambientVolumeIconMuted;
        [SerializeField] private Sprite _sfxVolumeIconMuted;

        private bool _editOptions = false;

        private void Awake()
        {
            _root.localScale = Vector3.zero;
        }

        private void Start()
        {
            _masterVolume.value = AudioManager.Instance.MasterVolume;
            _ambientVolume.value = AudioManager.Instance.AmbientVolume;
            _sfxVolume.value = AudioManager.Instance.SFXVolume;
        }

        private void OnEnable()
        {
            ManageEventCallbacks(true);
        }

        private void OnDisable()
        {
            ManageEventCallbacks(false);
        }

        private void ManageEventCallbacks(bool subscribeListeners)
        {
            if (subscribeListeners)
            {
                _mainButtonResume.onClick.AddListener(ResumeGame);
                _mainButtonOptions.onClick.AddListener(ShowOptionsPanel);
                _mainButtonMainMenu.onClick.AddListener(ReloadGame);
                _mainButtonExit.onClick.AddListener(SceneLoader.Instance.ExitGame);

                _optionsButtonReturn.onClick.AddListener(ShowOptionsPanel);

                _optionsButtonToggleMaster.onClick.AddListener(ToggleMasterVolume);
                _optionsButtonToggleAmbient.onClick.AddListener(ToggleAmbientVolume);
                _optionsButtonToggleSFX.onClick.AddListener(ToggleSFXVolume);
                _masterVolume.onValueChanged.AddListener(delegate { AudioManager.Instance.MasterVolume = _masterVolume.value; });
                _ambientVolume.onValueChanged.AddListener(delegate { AudioManager.Instance.AmbientVolume = _ambientVolume.value; });
                _sfxVolume.onValueChanged.AddListener(delegate { AudioManager.Instance.SFXVolume = _sfxVolume.value; });
            }
            else
            {
                _mainButtonResume.onClick.RemoveListener(ResumeGame);
                _mainButtonOptions.onClick.RemoveListener(ShowOptionsPanel);
                _mainButtonMainMenu.onClick.RemoveListener(ReloadGame);
                _mainButtonExit.onClick.RemoveListener(SceneLoader.Instance.ExitGame);

                _optionsButtonReturn.onClick.RemoveListener(ShowOptionsPanel);

                _optionsButtonToggleMaster.onClick.RemoveListener(ToggleMasterVolume);
                _optionsButtonToggleAmbient.onClick.RemoveListener(ToggleAmbientVolume);
                _optionsButtonToggleSFX.onClick.RemoveListener(ToggleSFXVolume);
                _masterVolume.onValueChanged.RemoveAllListeners();
                _ambientVolume.onValueChanged.RemoveAllListeners();
                _sfxVolume.onValueChanged.RemoveAllListeners();
            }
        }

        // ---

        private void ResumeGame()
        {
            GameStateManager.Instance.SwitchState(GameStateManager.Instance.PreviousState, true, false);
        }

        private void ReloadGame()
        {
            SceneLoader.Instance.SwitchScene(SceneType.MainMenu);
            GameStateManager.Instance.SwitchState(GameStateManager.Instance.GameStates.MainMenu);
        }

        public void ShowPausePanel()
        {
            _root.DOScale(1, 1f).SetEase(Ease.OutBounce);
        }

        public void HidePausePanel()
        {
            _root.DOScale(0, 1f).SetEase(Ease.InBounce);
        }

        // ---

        public void ShowOptionsPanel()
        {
            if (!_editOptions)
            {
                _editOptions = true;
                _pausePanel.DOKill();
                _mainPanel.DOKill();
                _pausePanel.DOAnchorPosX(_pausePanel.TargetAnchoredPosX(-2f), _slideAnimationDuration).SetEase(_slideAnimationEasing);
                _mainPanel.DOAnchorPosX(_mainPanel.TargetAnchoredPosX(-2f), _slideAnimationDuration).SetEase(_slideAnimationEasing);
            }
            else
            {
                _editOptions = false;
                _pausePanel.DOKill();
                _mainPanel.DOKill();
                _pausePanel.DOAnchorPosX(_pausePanel.TargetAnchoredPosX(0f), _slideAnimationDuration).SetEase(_slideAnimationEasing);
                _mainPanel.DOAnchorPosX(_mainPanel.TargetAnchoredPosX(0f), _slideAnimationDuration).SetEase(_slideAnimationEasing);
            }
        }

        private void ToggleMasterVolume()
        {
            bool muted = !AudioManager.Instance.MasterVolumeMuted;

            AudioManager.Instance.MasterVolumeMuted = muted;
            _optionsButtonToggleMaster.image.sprite = muted ? _masterVolumeIconMuted : _masterVolumeIcon;
            _masterVolume.value = muted ? 0 : AudioManager.Instance.MasterVolume;
            _masterVolume.interactable = !muted;
        }

        private void ToggleAmbientVolume()
        {
            bool muted = !AudioManager.Instance.AmbientVolumeMuted;

            AudioManager.Instance.AmbientVolumeMuted = muted;
            _optionsButtonToggleAmbient.image.sprite = muted ? _ambientVolumeIconMuted : _ambientVolumeIcon;
            _ambientVolume.value = muted ? 0 : AudioManager.Instance.AmbientVolume;
            _ambientVolume.interactable = !muted;
        }

        private void ToggleSFXVolume()
        {
            bool muted = !AudioManager.Instance.SFXVolumeMuted;

            AudioManager.Instance.SFXVolumeMuted = muted;
            _optionsButtonToggleSFX.image.sprite = muted ? _masterVolumeIconMuted : _masterVolumeIcon;
            _sfxVolume.value = muted ? 0 : AudioManager.Instance.SFXVolume;
            _sfxVolume.interactable = !muted;
        }
    }
}
