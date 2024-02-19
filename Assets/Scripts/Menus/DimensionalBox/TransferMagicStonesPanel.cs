using CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer;
using CryptoQuest.UI.Tooltips.Events;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox
{
    public class TransferMagicStonesPanel : MonoBehaviour
    {
        [field: SerializeField] public UIMagicStoneList IngameList { get; set; }
        [field: SerializeField] public UIMagicStoneList InboxList { get; set; }
        [field: SerializeField] public ShowTooltipEvent ShowTooltipEventChannel { get; private set; }
    }
}