using CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox
{
    public class MagicStoneController : MonoBehaviour
    {
        [field: SerializeField] public UIMagicStoneList InGameMagicStoneList { get; set; }
        [field: SerializeField] public UIMagicStoneList DBoxMagicStoneList { get; set; }
    }
}