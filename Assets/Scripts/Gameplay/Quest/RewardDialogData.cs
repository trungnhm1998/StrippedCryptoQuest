using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Quest
{
    [CreateAssetMenu(menuName = "Crypto Quest/Battle/Reward")]
    public class RewardDialogData : ScriptableObject
    {
        [SerializeField] private List<LocalizedString> _items;
        public List<LocalizedString> Items { get => _items; }
    }
}
