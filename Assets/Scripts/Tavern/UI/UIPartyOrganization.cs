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
            UICharacterList.Rendered += HandleListInteractable;
        }

        public override void StateExited()
        {
            base.StateExited();
            UITavernItem.Pressed -= Transfer;
            UICharacterList.Rendered -= HandleListInteractable;
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

        /// <summary>
        /// This method will disable all the buttons of the right list if the left list has data,
        /// then select first button of the left list.
        /// <para/>
        /// If there is no data in the left list,
        /// the first button of the right list will be selected.
        /// </summary>
        private void HandleListInteractable()
        {
            if (_partyScrollContent.childCount > 0)
            {
                _gameListUi.SetInteractableAllButtons(false);
                StartCoroutine(_partyUi.CoSetDefaultSelection());
            }
            else
            {
                StartCoroutine(_gameListUi.CoSetDefaultSelection());
            }
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