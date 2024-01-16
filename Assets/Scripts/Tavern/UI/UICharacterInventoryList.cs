using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UICharacterInventoryList : MonoBehaviour
    {
        public event Action<UICharacterListItemWithPartyIndicator> ItemSelected;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private HeroInventorySO _inventory;
        [SerializeField] private PartySO _party;
        [SerializeField] private UICharacterListItemWithPartyIndicator _itemPrefab;
        private readonly List<UICharacterListItemWithPartyIndicator> _items = new();

        private void OnEnable()
        {
            foreach (var item in _items)
            {
                Destroy(item.gameObject);
            }

            _items.Clear();

            var selectables = new List<Selectable>();
            foreach (var item in _inventory.OwnedHeroes)
            {
                var uiItem = Instantiate(_itemPrefab, _scrollRect.content);
                uiItem.Init(item);
                uiItem.MarkAsInParty(_party.Exists(item));
                var button = uiItem.GetComponent<Button>();
                selectables.Add(button);
                button.onClick.AddListener(() => OnSelectCharacter(uiItem));
                _items.Add(uiItem);
            }

            for (var index = 0; index < selectables.Count; index++)
            {
                var selectable = selectables[index];
                selectable.navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnUp = index > 0 ? selectables[index - 1] : null,
                    selectOnDown = index < selectables.Count - 1 ? selectables[index + 1] : null
                };
            }
        }

        private void OnSelectCharacter(UICharacterListItemWithPartyIndicator uiCharacterListItem)
        {
            if (uiCharacterListItem.IsMarkedAsInParty()) return;
            ItemSelected?.Invoke(uiCharacterListItem);
            uiCharacterListItem.MarkAsInParty(_party.Exists(uiCharacterListItem.Spec));
        }

        public void Refresh()
        {
            foreach (var item in _items)
            {
                item.MarkAsInParty(_party.Exists(item.Spec));
            }
        }
    }
}