using CryptoQuest.Item.MagicStone;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public interface ITooltipStoneProvider
    {
        public IMagicStone MagicStone { get; }
    }

    public class TooltipStoneProvider : MonoBehaviour, ITooltipStoneProvider
    {
        [SerializeField] private UISingleStone _uiSingleStone;
        public IMagicStone MagicStone => _uiSingleStone.Data;
    }
}