using CryptoQuest.Map;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Menus.TownTransfer
{
    public class TownTransferPath : MapPathSO
    {
        [field: SerializeField] public LocalizedString MapName { get; private set; }
    }
}