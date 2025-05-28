using System;
using UnityEngine;

namespace CodeBase.Scripts.Utils
{
    public static class VectorExtensions
    {
        public static bool Approximately(this Vector3 a, Vector3 b, float tolerance = float.Epsilon)
        {
            return (Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance && Math.Abs(a.z - b.z) < tolerance);
        }

        public static bool Approximately(this Vector2 a, Vector2 b, float tolerance = float.Epsilon)
        {
            return (Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance);
        }
    }
}
