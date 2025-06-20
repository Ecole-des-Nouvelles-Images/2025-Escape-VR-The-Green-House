using System;
using System.Diagnostics;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace Code.Scripts.Utils
{
    public static class CustomLogger
    {
        [Conditional("UNITY_EDITOR")]
        public static void LogInfo(string msg)
        {
            Debug.Log(msg);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(string msg)
        {
            Debug.LogWarning(msg);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(string msg)
        {
            Debug.LogError(msg);
        }

        /// <summary>
        /// Calls Unity's <c>Debug.Assert</c> and logs an error message if the assertion fails.<br/>
        /// <b>The function should compile in Editor only.</b><br/>
        /// </summary>
        /// <param name="condition">Condition expression that is expected to be true.</param>
        /// <param name="msg">Log's message as it will appear on Unity/Rider consoles.</param>
        /// <param name="throwOnFailure">Should the function throw on assertion's failure?</param>
        /// <exception cref="AssertionException"></exception>
        [Conditional("UNITY_EDITOR"), Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, string msg, bool throwOnFailure = false)
        {
            Debug.Assert(condition, msg);

            if (throwOnFailure & !condition) throw new AssertionException("[CustomLogger assertion failed]", msg);
        }

        /// <summary>
        /// Editor-only function that throws an Exception.<br/>
        /// <b>The function should compile in Editor only.</b>
        /// </summary>
        /// <param name="msg">Message as it will appear in Unity/Rider consoles.</param>
        /// <typeparam name="T">The type of object to throw. <b><em>Must inherit from Exception.</em></b></typeparam>
        [Conditional("UNITY_EDITOR")]
        public static void Raise<T>(string msg) where T: Exception, new()
        {
            T exception = (T) Activator.CreateInstance(typeof(T), msg);

            throw exception;
        }
    }
}
