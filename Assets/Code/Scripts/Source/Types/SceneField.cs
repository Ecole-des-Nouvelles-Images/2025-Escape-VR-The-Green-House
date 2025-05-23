using System;
using Code.Scripts.Source.Types;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Code.Scripts.Utils
{
    [Serializable]
    public class SceneField
    {
        [SerializeField] private Object _sceneAsset;

        public string SceneName { get; private set; }
        public string ScenePath { get; private set; }

        public SceneType SceneType {
            get
            {
                return SceneName switch
                {
                    "MainMenu" or "Main Menu" => SceneType.MainMenu,
                    "Hall" => SceneType.Hall,
                    "Lounge" => SceneType.Lounge,
                    "Greenhouse" => SceneType.Greenhouse,
                    "Laboratory" => SceneType.Laboratory,

                    _ => SceneType.Invalid
                };
            }
        }

        public SceneField(Object asset)
        {
            _sceneAsset = asset;
            SceneName = asset.name;

            if (SceneType == SceneType.Invalid)
                Debug.LogError($"Unable to determine the SceneType of scene: {SceneName}. " + $"Verify that scene '{SceneName}' is correctly named.");
        }

        public static implicit operator string(SceneField sceneField)
        {
            return sceneField.SceneName;
        }

        public static implicit operator Scene(SceneField sceneField)
        {
            return SceneManager.GetSceneByName(sceneField.SceneName);
        }

        public override string ToString()
        {
            return SceneName;
        }

#if UNITY_EDITOR
        public static implicit operator SceneField(Scene scene)
        {
            return new SceneField (AssetDatabase.LoadAssetAtPath<Object>(scene.path));
        }

        public static implicit operator SceneField(string sceneName)
        {
            string path = SceneManager.GetSceneByName(sceneName).path;
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);

            return new SceneField (asset);
        }
#endif

    }
}
