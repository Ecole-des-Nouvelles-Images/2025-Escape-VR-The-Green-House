using System.Collections.Generic;
using Code.Scripts.Source.Audio;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Source.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private CanvasGroup _mainMenuPanel;
        [SerializeField] private CanvasGroup _optionsPanel;
        [SerializeField] private CanvasGroup _creditsPanel;

        [Header("Main Menu Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private List<Button> _returnButtons;

        [Header("Options")]
        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _ambientVolume;
        [SerializeField] private Slider _sfxVolume;
        [Space]
        [SerializeField] private Button _optionsButtonToggleMaster;
        [SerializeField] private Button _optionsButtonToggleAmbient;
        [SerializeField] private Button _optionsButtonToggleSFX;
        [Space]
        [SerializeField] private Sprite _masterVolumeIcon;
        [SerializeField] private Sprite _ambientVolumeIcon;
        [SerializeField] private Sprite _sfxVolumeIcon;
        [SerializeField] private Sprite _masterVolumeIconMuted;
        [SerializeField] private Sprite _ambientVolumeIconMuted;
        [SerializeField] private Sprite _sfxVolumeIconMuted;

        [Header("Miscellaneous")]
        [SerializeField] private TMP_Text _appVersion;

        [Header("Animations")]
        [SerializeField] private float _fadeDuration = 1.5f;

        private void OnEnable()
        {
            ManageEventCallbacks(true);
        }

        private void OnDisable()
        {
            ManageEventCallbacks(false);
        }

        private void ManageEventCallbacks(bool subscribe)
        {
            if (subscribe)
            {
                _startButton.onClick.AddListener(StartGame);
                _optionsButton.onClick.AddListener(EnableOptionsPanel);
                _creditsButton.onClick.AddListener(EnableCreditsPanel);
                _quitButton.onClick.AddListener(QuitGame);

                foreach (Button item in _returnButtons)
                    item.onClick.AddListener(EnableMainMenuPanel);

                _optionsButtonToggleMaster.onClick.AddListener(ToggleMasterVolume);
                _optionsButtonToggleAmbient.onClick.AddListener(ToggleAmbientVolume);
                _optionsButtonToggleSFX.onClick.AddListener(ToggleSFXVolume);
                _masterVolume.onValueChanged.AddListener(delegate { AudioManager.Instance.MasterVolume = _masterVolume.value; });
                _ambientVolume.onValueChanged.AddListener(delegate { AudioManager.Instance.AmbientVolume = _ambientVolume.value; });
                _sfxVolume.onValueChanged.AddListener(delegate { AudioManager.Instance.SFXVolume = _sfxVolume.value; });
            }
            else
            {
                _startButton.onClick.RemoveListener(StartGame);
                _optionsButton.onClick.RemoveListener(EnableOptionsPanel);
                _creditsButton.onClick.RemoveListener(EnableCreditsPanel);
                _quitButton.onClick.RemoveListener(QuitGame);

                foreach (Button item in _returnButtons)
                    item.onClick.RemoveListener(EnableMainMenuPanel);

                _optionsButtonToggleMaster.onClick.RemoveListener(ToggleMasterVolume);
                _optionsButtonToggleAmbient.onClick.RemoveListener(ToggleAmbientVolume);
                _optionsButtonToggleSFX.onClick.RemoveListener(ToggleSFXVolume);
                _masterVolume.onValueChanged.RemoveAllListeners();
                _ambientVolume.onValueChanged.RemoveAllListeners();
                _sfxVolume.onValueChanged.RemoveAllListeners();
            }
        }

        // ---

        private void StartGame()
        {
            HideAllPanels();
            GameStateManager.Instance.SwitchState(GameStateManager.Instance.GameStates.HallIntro);
            SceneLoader.Instance.SwitchScene(SceneType.Hall);
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }

        private void HideAllPanels()
        {
            _mainMenuPanel.DOFade(0, _fadeDuration);
            _optionsPanel.DOFade(0, _fadeDuration);
            _creditsPanel.DOFade(0, _fadeDuration);
        }

        private void EnableMainMenuPanel()
        {
            _mainMenuPanel.DOFade(1, _fadeDuration);
            _optionsPanel.DOFade(0, _fadeDuration);
            _creditsPanel.DOFade(0, _fadeDuration);
        }

        private void EnableOptionsPanel()
        {
            _mainMenuPanel.DOFade(0, _fadeDuration);
            _optionsPanel.DOFade(1, _fadeDuration);
            _creditsPanel.DOFade(0, _fadeDuration);
        }

        private void EnableCreditsPanel()
        {
            _mainMenuPanel.DOFade(0, _fadeDuration);
            _optionsPanel.DOFade(0, _fadeDuration);
            _creditsPanel.DOFade(1, _fadeDuration);
        }

        // ---

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
