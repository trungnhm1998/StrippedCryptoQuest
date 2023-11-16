using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public enum ETooltipType
    {
        Consumable = 0,
        Equipment = 1,
        Character = 2,
        Beast = 3,
    }

    public class TooltipFactory : MonoBehaviour
    {
        public static TooltipFactory Instance;

        [SerializeField] private List<UITooltip> _tooltips;

        private readonly Dictionary<ETooltipType, UITooltip> _caches = new();

        private void Awake()
        {
            Instance = this;

            foreach (var tooltip in _tooltips)
            {
                _caches.Add(tooltip.Type, tooltip);
            }
        }

        public ITooltip GetTooltip(ETooltipType tooltipType)
        {
            return _caches[tooltipType];
        }
    }
}