using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Quest
{
    [CreateAssetMenu(menuName = "Crypto Quest/Battle/Level Status Data")]
    public class LevelStatusDialogData : ScriptableObject
    {
        [field: SerializeField] public List<LocalizedString> TargetTextList { get; private set; }
        public bool IsValid() => TargetTextList is { Count: > 0 };
        
        public void TargetIsLevelUp(LocalizedString target)
        {
            TargetTextList.Add(target);
        }

        public void ClearListTarget()
        {
            TargetTextList.Clear();
        }
    }
}
