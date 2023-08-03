using UnityEngine;

namespace CryptoQuest.Data
{
    public class ItemGenericData : ScriptableObject
    {
        public int ID;

        [SerializeField] private string _displayName;
        public string DisplayName => _displayName;

        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;

        [TextArea(3, 10)] public string Description;

        public void SetIcon(Sprite icon)
        {
            this._icon = icon;
        }
    }
}