using System;
using System.Collections.Generic;
using Code.Scripts.Source.Audio;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using DG.Tweening;

using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;
using Code.Scripts.Source.UI;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStatePause : GameBaseState
    {
        [SerializeField] private PauseMenuController _pauseUI;
        [Space]
        [SerializeField] private Volume _postRenderVolume;
        [SerializeField] private float _exposureAnimationDuration = 2f;
        [SerializeField] [Range(-10, 0)] private float _exposureToneDownIntensity = -2f;

        private ColorAdjustments _colorAdjustorModule;

        private List<XRBaseInteractable> _currentSceneInteractable = new ();

        // ---

        public GameStatePause()
        {
            SceneLoader.OnSceneChanged += ResetActiveXRInteractables;
        }

        ~GameStatePause()
        {
            SceneLoader.OnSceneChanged -= ResetActiveXRInteractables;
        }

        // ---

        public override void EnterState(GameStateManager context)
        {
            base.EnterState(context);

            context.GamePaused = true;

            context.ChangeNearFarInteractionMode(NearFarMode.Far);

            if (!_postRenderVolume)
                throw new NullReferenceException("[GameStatePause] Missing reference for _postRenderVolume.\n> Check if the post-process volume is assigned in the inspector.");

            if (!_postRenderVolume.profile.TryGet(out _colorAdjustorModule))
                throw new Exception("[GameStatePause] Unable to get {ColorAdjustments} post-process profile's module");

            UpdateCurrentXRInteractables();
            EnableXRInteractable(false);

            DOTween.To(
                () => _colorAdjustorModule.postExposure.value,
                x => _colorAdjustorModule.postExposure.value = x,
                _exposureToneDownIntensity, _exposureAnimationDuration
            );

            _pauseUI.ShowPausePanel();
            AudioManager.Instance.SetLowpassFrequency(400f);


        }

        public override void UpdateState(GameStateManager context)
        {
            if (context.MenuButton.WasPressedThisFrame() || context.MenuButtonInteraction.WasPressedThisFrame())
            {
                context.SwitchState(context.PreviousState, true, false);
            }
        }

        public override void ExitState(GameStateManager context)
        {
            _pauseUI.HidePausePanel();
            AudioManager.Instance.SetLowpassFrequency(4000f);

            DOTween.To(
                () => _colorAdjustorModule.postExposure.value,
                x => _colorAdjustorModule.postExposure.value = x,
                0, _exposureAnimationDuration
            );

            context.GamePaused = false;
            EnableXRInteractable(true);

            context.ChangeNearFarInteractionMode(NearFarMode.Near);

            Debug.Log("[GameStatePause] Exiting...");
        }

        // ---

        private void EnableXRInteractable(bool enable)
        {
            if (_currentSceneInteractable == null || _currentSceneInteractable.Count <= 0)
            {
                Debug.LogWarning($"GameStatePause: No XRBaseInteractable found in scene {SceneLoader.Instance.CurrentScene} but no further problems to report.");
                return;
            }

            for (int i = 0; i < _currentSceneInteractable.Count; i++)
                _currentSceneInteractable[i].enabled = enable;
        }

        private void UpdateCurrentXRInteractables()
        {
            if (_currentSceneInteractable.Count == 0)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                GameObject[] activeObjects = activeScene.GetRootGameObjects();

                foreach (GameObject item in activeObjects)
                {
                    XRBaseInteractable[] interactableCollection = item.GetComponentsInChildren<XRBaseInteractable>();

                    if (interactableCollection is { Length: > 0 })
                    {
                        _currentSceneInteractable.AddRange(interactableCollection);
                    }
                }
            }
        }

        public void ResetActiveXRInteractables()
        {
            _currentSceneInteractable.Clear();
        }
    }
}
