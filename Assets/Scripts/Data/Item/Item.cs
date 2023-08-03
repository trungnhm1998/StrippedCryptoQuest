using UnityEngine;

namespace CryptoQuest.Data.Item
{
    public class Item : ItemGeneric
    {
        public Sprite PreviewSprite;

        [SerializeField] protected ItemTypeSO _itemTypeSo;
        public ItemTypeSO ItemTypeSo => _itemTypeSo;

        public virtual void Activate() { }
    }
}