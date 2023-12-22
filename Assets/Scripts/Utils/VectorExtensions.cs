using UnityEngine;

namespace CryptoQuest.Utils
{
    public static class Vector3DebugExtensions
    {
        public static void DrawSphere(this Vector3 position, float radius, Color color = default)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(position, radius);
        }

        public static void DrawWireSphere(this Vector3 position, float radius, Color color = default)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(position, radius);
        }
    }
}
