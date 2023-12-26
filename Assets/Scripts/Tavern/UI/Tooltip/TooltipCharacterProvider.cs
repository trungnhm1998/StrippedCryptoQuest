using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Tavern.UI.Tooltip
{
    public class TooltipCharacterProvider : MonoBehaviour, ITooltipHeroProvider
    {
        [SerializeField] private UITavernItem _uiTavernItem;
        public HeroSpec Hero => _uiTavernItem.Hero;
    }
}