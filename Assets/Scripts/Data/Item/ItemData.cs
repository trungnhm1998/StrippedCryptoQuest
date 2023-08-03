using UnityEngine;

namespace CryptoQuest.Data.Item
{
    public class ItemData : ItemGenericData
    {
        public Sprite PreviewSprite;

        [SerializeField] protected ItemTypeSO _itemTypeSo;
        public ItemTypeSO ItemTypeSo => _itemTypeSo;

        public virtual void Activate() { }

#if UNITY_EDITOR

        public void SetItemTypeSO(ItemTypeSO itemTypeSO)
        {
            this._itemTypeSo = itemTypeSO;
        }

#endif
    }
}