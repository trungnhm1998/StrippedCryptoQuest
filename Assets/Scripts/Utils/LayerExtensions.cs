using UnityEngine;

namespace CryptoQuest.Utils
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return ((mask & (1 << layer)) != 0);
        }

        public static LayerMask Add(this LayerMask mask, int layer)
        {
            return mask | layer;
        }

        public static LayerMask Remove(this LayerMask mask, int layer)
        {
            return mask & ~layer;
        }
    }
}
