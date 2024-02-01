using CryptoQuest.UI.Tooltips.Events;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class MagicStoneSelection : MonoBehaviour
    {
        [field: SerializeField] public UIStoneList UIStoneList { get; private set; }
        [SerializeField] private Button[] _stoneTypes;
        [SerializeField] private ShowTooltipEvent _showTooltipEvent;

        public void EnterElementNavigation()
        {
            SetActiveAllElementButtons(true);
            _stoneTypes[0].Select();
        }

        public void EnterStoneSelection()
        {
            SetActiveAllElementButtons(false);
            UIStoneList.SetActiveAllStoneButtons(true);
        }

        public void ExitStoneSelection()
        {
            UIStoneList.SetActiveAllStoneButtons(false);
        }

        public void SetActiveAllElementButtons(bool enable)
        {
            foreach (var type in _stoneTypes)
                type.enabled = enable;
        }

        public void DeactivateTooltip()
        {
            _showTooltipEvent.RaiseEvent(false);
        }
    }
}