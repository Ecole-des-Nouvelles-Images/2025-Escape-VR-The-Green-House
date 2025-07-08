using System;
using UnityEngine;

namespace Code.Scripts.Utils
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    T[] objs = FindObjectsByType<T>(FindObjectsSortMode.None);

                    if (objs.Length > 0)
                        _instance = objs[0];

                    if (objs.Length > 1)
                        CustomLogger.Raise<Exception>($"[{typeof(T)}] There is more than one instance in the scene !");
                }

                if (!_instance)
                {
                    CustomLogger.Raise<Exception>($"No singleton instance of {typeof(T)} found in the scene");
                }

                return _instance;
            }
        }

        // private static void OnRuntimeInitialize()
        // {
        //     _instance = FindFirstObjectByType<T>();
        // }
    }
}
