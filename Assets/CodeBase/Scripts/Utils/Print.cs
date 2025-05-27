using UnityEngine;

namespace CodeBase.Scripts.Utils
{
    public static class Print
    {
        public static void Log<T>(T text)
        {
#if UNITY_EDITOR
            Debug.Log(text.ToString());
#endif
        }

        public static void LogWarning<T>(T text)
        {
#if UNITY_EDITOR
            Debug.LogWarning(text.ToString());
#endif
        }

        public static void LogError<T>(T text)
        {
#if UNITY_EDITOR
            Debug.LogError(text.ToString());
#endif
        }
    }
}
