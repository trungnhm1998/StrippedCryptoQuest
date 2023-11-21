using CryptoQuest.Map;
using UnityEngine;
using UnityEngine.Localization;

namespace TownTransfer
{
    public class TownTransferPath : MapPathSO
    {
        [field: SerializeField] public LocalizedString MapName { get; private set; }
    }
}