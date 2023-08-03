using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Data
{
    public class ItemGeneric : ScriptableObject
    {
        public int ID;

        [SerializeField] private LocalizedString _displayName;
        public LocalizedString DisplayName => _displayName;
        public LocalizedString Description;

        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;

        [SerializeField] private bool _isNftItem;
        public bool IsNftItem => _isNftItem;

        public void SetIcon(Sprite icon)
        {
            this._icon = icon;
        }
    }
}