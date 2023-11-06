using System.Collections.Generic;
using CryptoQuest.Tavern.UI.CharacterReplacement;
using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public class UIPartyOrganization : UIAbstractTavern
    {
        [SerializeField] private UICharacterList _partyUi;
        [SerializeField] private UICharacterList _gameListUi;

        [SerializeField] private Transform _partyScrollContent;
        [SerializeField] private Transform _gameScrollContent;
        
        private List<int> _selectedGameItemsIds = new();
        public List<int> SelectedGameItemsIds { get => _selectedGameItemsIds; private set => _selectedGameItemsIds = value; }

        private List<int> _selectedWalletItemsIds = new();
        public List<int> SelectedWalletItemsIds { get => _selectedWalletItemsIds; private set => _selectedWalletItemsIds = value; }

        public override void StateEntered()
        {
            base.StateEntered();
            UITavernItem.Pressed += Transfer;
            _gameListUi.SetInteractableAllButtons(true);
        }

        public override void StateExited()
        {
            base.StateExited();
            UITavernItem.Pressed -= Transfer;
        }

        private void Transfer(UITavernItem currentItem)
        {
            _selectedGameItemsIds.Clear();
            _selectedWalletItemsIds.Clear();

            var currentList = currentItem.Parent;

            Transform otherList;
            if (currentList == _gameScrollContent)
            {
                otherList = _partyScrollContent;
                _selectedGameItemsIds.Add(currentItem.Id);
            }
            else
            {
                otherList = _gameScrollContent;
                _selectedWalletItemsIds.Add(currentItem.Id);
            }

            currentItem.Transfer(otherList);

            _partyUi.SetInteractableAllButtons(otherList == _partyScrollContent);
            _gameListUi.SetInteractableAllButtons(otherList == _gameScrollContent);
        }

        public void SwitchList(Vector2 direction)
        {
            if (_partyScrollContent.childCount <= 0) return;
            if (_gameScrollContent.childCount <= 0) return; // code smells

            switch (direction.x)
            {
                case > 0:
                    _partyUi.SetInteractableAllButtons(false);
                    FocusList(_gameListUi);
                    break;
                case < 0:
                    _gameListUi.SetInteractableAllButtons(false);
                    FocusList(_partyUi);
                    break;
            }
        }

        private void FocusList(UICharacterList targetList)
        {
            targetList.SetInteractableAllButtons(true);
            StartCoroutine(targetList.CoSetDefaultSelection());
        }

        public void CheckEmptyList(UICharacterList target, bool isGameListEmpty)
        {
            if (!isGameListEmpty)
                target.SetInteractableAllButtons(false);
        }

        public void ConfirmedTransmission()
        {
            _partyUi.UpdateList();
            _gameListUi.UpdateList();
        }
    }
}