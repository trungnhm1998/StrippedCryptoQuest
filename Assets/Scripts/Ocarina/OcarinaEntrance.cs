using CryptoQuest.Map;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ocarina
{
    public class OcarinaEntrance : MapPathSO
    {
        [field: SerializeField] public LocalizedString MapName { get; private set; }
    }
}