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
                if (_instance == null)
                {
                    T[] objs = FindObjectsByType<T>(FindObjectsSortMode.None);

                    if (objs.Length > 0)
                        _instance = objs[0];

                    if (objs.Length > 1)
                        throw new Exception($"[{typeof(T).Name}] There is more than one instance in the scene !");
                }

                if (!_instance)
                {
                    throw new Exception($"[{typeof(T).Name}] No singleton instance found in the scene !");
                }

                return _instance;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeInitialize()
        {
            _instance = FindFirstObjectByType<T>();
        }
    }
}
