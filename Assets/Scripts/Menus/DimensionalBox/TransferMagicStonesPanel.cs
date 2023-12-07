using CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox
{
    public class TransferMagicStonesPanel : MonoBehaviour
    {
        [field: SerializeField] public UIMagicStoneList IngameList { get; set; }
        [field: SerializeField] public UIMagicStoneList InboxList { get; set; }
    }
}