using System.Collections.Generic;
using CryptoQuest.Sagas.Character;
using CryptoQuest.UI.Extensions;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.UI;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Tavern.UI
{
    public class UICharacterList : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UICharacterListItem _itemPrefab;
        private bool _interactable;

        public bool Interactable
        {
            set
            {
                _interactable = value;
                var buttons = _scrollRect.content.GetComponentsInChildren<Button>();
                foreach (var button in buttons) button.interactable = value;
            }
        }

        public void Init(List<Obj.Character> characterResponses)
        {
            _scrollRect.content.DestroyAllChildrenImmediately();
            var responseConverter = ServiceProvider.GetService<IHeroResponseConverter>();
            foreach (var characterResponse in characterResponses)
            {
                var heroSpec = responseConverter.Convert(characterResponse);
                if (heroSpec.Origin == null) continue;
                var uiItem = Instantiate(_itemPrefab, _scrollRect.content);
                uiItem.Init(heroSpec);
            }

            SetupNavigationAndTag(characterResponses);
        }

        private void SetupNavigationAndTag(List<Obj.Character> characterResponses)
        {
            var buttons = _scrollRect.content.GetComponentsInChildren<Button>();
            for (var index = 0; index < buttons.Length; index++)
            {
                var button = buttons[index];
                button.navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnUp = index > 0 ? buttons[index - 1] : buttons[^1],
                    selectOnDown = index < buttons.Length - 1 ? buttons[index + 1] : buttons[0]
                };
                var transferableComponent = button.GetComponent<UITransferable>();
                transferableComponent.IsTransferring = characterResponses[index].IsTransferring;
                if (transferableComponent.IsTransferring) continue;
                button.onClick.AddListener(() => transferableComponent.TogglePending());
            }
        }
        
        public List<UICharacterListItem> GetSelectedItems()
        {
            var selectedItems = new List<UICharacterListItem>();
            var transferableItems = _scrollRect.content.GetComponentsInChildren<UITransferable>();
            foreach (var transferableItem in transferableItems)
            {
                if (transferableItem.IsPending) selectedItems.Add(transferableItem.GetComponent<UICharacterListItem>());
            }

            return selectedItems;
        }
    }
}