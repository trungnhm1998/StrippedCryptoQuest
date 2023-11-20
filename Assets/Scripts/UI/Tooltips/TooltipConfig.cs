using System;
using UnityEngine;

namespace CryptoQuest.UI.Tooltips
{
    public class TooltipConfig : MonoBehaviour
    {
        [Serializable]
        public struct Config
        {
            public Vector2 Pivot;
            public Vector2 Offset;
            [SerializeField] private RectTransform _positionTransform;
            public Vector2 Position => _positionTransform.position;
        }

        [field: SerializeField] public Config Default { get; private set; } = new() { Pivot = new Vector2(0f, 1f) };
    }
}