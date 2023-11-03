using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class UICharacterReplacement : UIAbstractTavern
    {
        [SerializeField] private UICharacterList _gameListUi;
        [SerializeField] private UICharacterList _walletListUi;

        [SerializeField] private Transform _gameScrollContent;
        [SerializeField] private Transform _walletScrollContent;

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
                otherList = _walletScrollContent;
                _selectedGameItemsIds.Add(currentItem.Id);
            }
            else
            {
                otherList = _gameScrollContent;
                _selectedWalletItemsIds.Add(currentItem.Id);
            }

            currentItem.Transfer(otherList);

            _gameListUi.SetInteractableAllButtons(otherList == _gameScrollContent);
            _walletListUi.SetInteractableAllButtons(otherList == _walletScrollContent);

            
        }

        public void SwitchList(Vector2 direction)
        {
            if (_gameScrollContent.childCount <= 0) return;
            if (_walletScrollContent.childCount <= 0) return; // code smells

            switch (direction.x)
            {
                case > 0:
                    _gameListUi.SetInteractableAllButtons(false);
                    FocusList(_walletListUi);
                    break;
                case < 0:
                    _walletListUi.SetInteractableAllButtons(false);
                    FocusList(_gameListUi);
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
            _gameListUi.UpdateList();
            _walletListUi.UpdateList();
        }

    }
}