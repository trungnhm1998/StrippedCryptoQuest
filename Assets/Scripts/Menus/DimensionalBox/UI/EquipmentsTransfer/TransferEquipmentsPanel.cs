using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer
{
    public class TransferEquipmentsPanel : MonoBehaviour
    {
        [field: SerializeField] public UIEquipmentList IngameList { get; private set; }
        [field: SerializeField] public UIEquipmentList InboxList { get; private set; }
    }
}