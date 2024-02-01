using CryptoQuest.UI.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class SingleStoneTooltipTrigger : MonoBehaviour
    {
        [SerializeField] private TooltipTrigger _tooltipTrigger;

        public void OnInspect()
        {
            if (_tooltipTrigger != null)
                _tooltipTrigger.OnSelect(null);
        }
    }
}