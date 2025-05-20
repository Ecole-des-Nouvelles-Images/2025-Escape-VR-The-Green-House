using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Source.Types;
using UnityEngine;
using UnityEngine.SceneManagement;

using Code.Scripts.Utils;
using UnityEditor;

namespace Code.Scripts.Source.Managers
{
    public class SceneLoader : MonoBehaviourSingleton<SceneLoader>
    {
        // [SerializeField] private List<Object> _scenes; // TODO: SceneField low-level ctor error.
        [SerializeField] private List<string> _scenes;

        [Header("Transition settings")]
        [SerializeField] private float _fadeDuration = 2f;
        [SerializeField] private AnimationCurve _fadeCurve;

        // public static Dictionary<SceneType, SceneField> SceneAssets { get; private set; } = new(); // TODO: SceneField low-level ctor error.
        public static Dictionary<SceneType, string> SceneAssets { get; private set; } = new();

        // public SceneField CurrentScene; // TODO: SceneField low-level ctor error.
        public string CurrentScene { get; private set; }
        public string PreviousScene { get; private set; }

        private SceneTransitionManager _transitionManager;

#if UNITY_EDITOR
        private void OnValidate()
        {
            BuildSceneDatabase();
        }
#endif

        private void Awake()
        {
            BuildSceneDatabase();
            _transitionManager = new(_fadeDuration, _fadeCurve);
        }

        private void Start()
        {
            LoadScene(SceneType.MainMenu);
            GameStateManager.Instance.OnFirstSceneLoaded.Invoke();
        }

        private void BuildSceneDatabase()
        {
            SceneAssets.Clear();

            foreach (string scene in _scenes)
            {
                SceneType type;

                switch (scene) {
                    case "Main Menu":  type = SceneType.MainMenu;   break;
                    case "Hall":       type = SceneType.Hall;       break;
                    case "Lounge":     type = SceneType.Lounge;     break;
                    case "Greenhouse": type = SceneType.Greenhouse; break;
                    case "Laboratory": type = SceneType.Laboratory; break;
                    case "CorridorA": type = SceneType.CorridorA; break;
                    case "CorridorB": type = SceneType.CorridorB; break;

                    default: type = SceneType.Invalid; break;
                }

                SceneAssets.Add(type, scene);
            }
        }

        /*private void BuildSceneDatabase() // TODO: SceneField low-level ctor error.
        {
            string debug = "";
            SceneAssets.Clear();

            foreach (Object item in _scenes)
            {
                SceneField sceneAsset = new(item);
                SceneAssets.TryAdd(sceneAsset.SceneType, sceneAsset);
                debug += $"→ {sceneAsset.SceneName}\n";
            }

            Debug.Log("Scene index database built:\n" + debug);
        }*/

        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }


        /// <summary>
        /// Start loading a scene asynchronously through a coroutine.
        /// </summary>
        /// <param name="sceneType">
        /// The <c>SceneType</c> of the necessary scene, as stored into the statically available Dictionary "SceneAssets".
        /// </param>
        /// <param name="loadMode"><c>LoadSceneMode.Additive</c> by default. Used in rare cases</param>
        public void LoadScene(SceneType sceneType, LoadSceneMode loadMode = LoadSceneMode.Additive)
        {
            StartCoroutine(LoadSceneCoroutine(SceneAssets[sceneType], true, loadMode, _fadeDuration));
        }


        /// <summary>
        /// Simple call to <c>SceneManager.LoadScene()</c> to load another scene manually, outside the game scene workflow.
        /// </summary>
        /// <param name="sceneName">Scene name to load</param>
        /// <param name="loadMode">Mandatory parameter when a manual load. <c>LoadSceneMode.Additive</c> by default.
        /// Used in rare cases</param>
        public void LoadSceneManual(string sceneName, LoadSceneMode loadMode)
        {
            SceneManager.LoadScene(sceneName, loadMode);
        }


        /// <summary>
        /// Switch asynchronously to a new scene through a coroutine, unloading the current one before loading the new one.<br/>
        /// Also, fully plays the transition animations.
        /// </summary>
        /// <param name="sceneType">The <c>SceneType</c> of the necessary scene, as stored into the statically available Dictionary "SceneAssets".</param>
        public void SwitchScene(SceneType sceneType)
        {
            StartCoroutine(SwitchLoadedScene(SceneAssets[sceneType]));
        }


        /// <summary>
        /// Wait for the scene transition to complete and unloads the current scene before loading the specified one.
        /// </summary>
        /// <param name="scene"><c>SceneField</c> wrapper or convertable scene name string or <c>Scene</c> object.</param>
        /// <param name="loadAsActive">Should the scene be immediately marked as active when loaded or not. <c>true</c> by default.</param>
        /// <returns></returns>
        private IEnumerator SwitchLoadedScene(/*SceneField scene*/string scene, bool loadAsActive = true)
        {
            _transitionManager.Crossfade.FadeIn();

            yield return new WaitForSeconds(_fadeDuration);

            if (CurrentScene != null)
                yield return UnloadSceneCoroutine(CurrentScene);

            yield return LoadSceneCoroutine(scene, loadAsActive);
        }


        /// <summary>
        /// Load asynchronously the specified scene additively and call the "out" transition.
        /// </summary>
        /// <param name="scene"><c>SceneField</c> wrapper or convertable scene name string or <c>Scene</c> object.</param>
        /// <param name="loadAsActive">Should the scene be immediately marked as active when loaded or not. <c>true</c> by default.</param>
        /// <param name="loadMode"><c>LoadSceneMode.Additive</c> by default. Used in rare cases</param>
        /// <param name="minimumLoadTime">Optional delay before enabling the scene.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Throws if an invalid scene has been specified.</exception>
        private IEnumerator LoadSceneCoroutine(/*SceneField scene*/string scene, bool loadAsActive = true, LoadSceneMode loadMode = LoadSceneMode.Additive, float minimumLoadTime = 0f)
        {
            yield return new WaitForSeconds(minimumLoadTime);

            AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(scene, loadMode);

            if (asyncLoadOperation == null)
                throw new NullReferenceException($"LoadSceneAsync error: {scene} scene is null.");

            while (!asyncLoadOperation.isDone)
                yield return null;

            CurrentScene = scene;
            asyncLoadOperation.allowSceneActivation = true;

            if (loadAsActive) {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
                Debug.Log($"[SceneLoader] Scene {{{SceneManager.GetActiveScene().name}}} is now active.");
            }
            _transitionManager.Crossfade.FadeOut();
        }


        /// <summary>
        /// Unload asynchronously the specified scene.
        /// </summary>
        /// <param name="scene"><c>SceneField</c> wrapper or convertable scene name string or <c>Scene</c> object.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Throws if an invalid scene has been specified.</exception>
        private IEnumerator UnloadSceneCoroutine(/*SceneField scene*/ string scene)
        {
            AsyncOperation asyncUnloadOperation = SceneManager.UnloadSceneAsync(scene);

            if (asyncUnloadOperation == null)
                throw new NullReferenceException($"UnloadSceneAsync error: {scene} scene is null.");

            while (!asyncUnloadOperation.isDone)
            {
                yield return null;
            }
        }
    }
}
