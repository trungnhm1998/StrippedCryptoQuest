using CryptoQuest.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UICharacterInventoryList : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private HeroInventorySO _inventory;
        [SerializeField] private UITavernItem _itemPrefab;

        private void OnEnable()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var item in _inventory.OwnedHeroes)
            {
                var uiItem = Instantiate(_itemPrefab, _scrollRect.content);
                uiItem.SetItemInfo(item);
            }
        }

        private void OnDisable() { }
    }
}