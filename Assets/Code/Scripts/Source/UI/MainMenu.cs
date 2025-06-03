using System.Collections.Generic;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Source.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private GameObject _creditsPanel;

        [Header("Main Menu Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _quitButton;

        [SerializeField] private List<Button> _returnButtons;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartGame);
            _optionsButton.onClick.AddListener(EnableOptionsPanel);
            _creditsButton.onClick.AddListener(EnableCreditsPanel);
            _quitButton.onClick.AddListener(QuitGame);

            foreach (Button item in _returnButtons)
                item.onClick.AddListener(EnableMainMenuPanel);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartGame);
            _optionsButton.onClick.RemoveListener(EnableOptionsPanel);
            _creditsButton.onClick.RemoveListener(EnableCreditsPanel);
            _quitButton.onClick.RemoveListener(QuitGame);

            foreach (Button item in _returnButtons)
                item.onClick.RemoveListener(EnableMainMenuPanel);
        }

        private void StartGame()
        {
            HideAllPanels();
            GameStateManager.Instance.SwitchState(GameStateManager.Instance.GameStates.Launch);
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
            _mainMenuPanel.SetActive(false);
            _optionsPanel.SetActive(false);
            _creditsPanel.SetActive(false);
        }

        private void EnableMainMenuPanel()
        {
            _mainMenuPanel.SetActive(true);
            _optionsPanel.SetActive(false);
            _creditsPanel.SetActive(false);
        }

        private void EnableOptionsPanel()
        {
            _mainMenuPanel.SetActive(false);
            _optionsPanel.SetActive(true);
            _creditsPanel.SetActive(false);
        }

        private void EnableCreditsPanel()
        {
            _mainMenuPanel.SetActive(false);
            _optionsPanel.SetActive(false);
            _creditsPanel.SetActive(true);
        }
    }
}
