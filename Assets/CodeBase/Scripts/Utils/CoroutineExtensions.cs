using UnityEngine;

namespace CodeBase.Scripts.Utils
{
    public static class Extensions
    {
        public static void StopCoroutine(this MonoBehaviour behaviour, ref Coroutine coroutine)
        {
            if (coroutine == null)
                return;

            behaviour.StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
