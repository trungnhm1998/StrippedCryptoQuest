using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Data
{
    public class ItemGenericSO : ScriptableObject
    {
        [Header("Item Generic Data")]
        public int ID;

        [field: SerializeField] public LocalizedString DisplayName { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public bool IsNftItem { get; private set; }

        public void SetIcon(Sprite icon)
        {
            this.Icon = icon;
        }
    }
}