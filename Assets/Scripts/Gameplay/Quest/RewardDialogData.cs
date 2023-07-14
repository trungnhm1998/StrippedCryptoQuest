using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Quest
{
    [CreateAssetMenu(menuName = "Crypto Quest/Battle/Reward")]
    public class RewardDialogData : ScriptableObject
    {
        [field: SerializeField] public List<LocalizedString> ItemNames { get; private set; }
    }
}
