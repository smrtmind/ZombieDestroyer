using System.Text.RegularExpressions;
using UnityEngine;

namespace CodeBase.Scripts.Utils
{
    public static class StringHelper
    {
        public static string GetFormattedString<T>(T name, string intervalFiller = " ")
        {
            string[] words = Regex.Split(name.ToString(), @"(?<!^)(?=[A-Z])");
            return string.Join(intervalFiller, words);
        }

        public static string GetHexColor(Color color) => ColorUtility.ToHtmlStringRGB(color);
    }
}
