using CodeBase.Scripts.Utils;
using System.Collections;
using TMPro;
using UnityEngine;

namespace CodeBase.Scripts.UI
{
    public class TypeText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField, Min(0.01f)] private float delayBetweenType = 0.05f;

        private Coroutine _typeRoutine;

        public void StartAnimation()
            => _typeRoutine ??= StartCoroutine(StartType(text, delayBetweenType));

        private void StopAnimation() => this.StopCoroutine(ref _typeRoutine);

        private IEnumerator StartType(TMP_Text tmp, float typeDelay)
        {
            var richTextIndex = 0;
            var insideTag = false;
            var targetText = tmp.text;

            tmp.text = "";

            for (int i = 0; i < targetText.Length; i++)
            {
                tmp.text = targetText.Substring(0, richTextIndex + 1);

                if (targetText[i] == '<') insideTag = true;
                if (!insideTag) yield return new WaitForSecondsRealtime(typeDelay);
                if (targetText[i] == '>') insideTag = false;

                richTextIndex++;
            }
        }

        private void OnDisable()
        {
            StopAnimation();
        }
    }
}
