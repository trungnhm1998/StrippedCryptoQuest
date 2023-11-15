using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public class UICharacterReplacement : UIAbstractTavern
    {
        [SerializeField] private UICharacterList _gameListUi;
        [SerializeField] private UICharacterList _walletListUi;

        [SerializeField] private Transform _gameScrollContent;
        [SerializeField] private Transform _walletScrollContent;

        private List<int> _selectedGameItemsIds = new();

        public List<int> SelectedGameItemsIds
        {
            get => _selectedGameItemsIds;
            private set => _selectedGameItemsIds = value;
        }

        private List<int> _selectedWalletItemsIds = new();

        public List<int> SelectedWalletItemsIds
        {
            get => _selectedWalletItemsIds;
            private set => _selectedWalletItemsIds = value;
        }

        public void Transfer(UITavernItem currentItem)
        {
            var currentList = currentItem.Parent;
            if (currentList == _gameScrollContent)
            {
                currentItem.Transfer(_walletScrollContent);
                _selectedGameItemsIds.Add(currentItem.Id);
            }
            else
            {
                currentItem.Transfer(_gameScrollContent);
                _selectedWalletItemsIds.Add(currentItem.Id);
            }

            _gameListUi.SetInteractableAllButtons(!(currentList == _gameScrollContent));
            _walletListUi.SetInteractableAllButtons(currentList == _gameScrollContent);

            Debug.Log($"game={_selectedGameItemsIds.Count} -- wallet={_selectedWalletItemsIds.Count}");
        }

        /// <summary>
        /// This method will disable all the buttons of the right list if the left list has data,
        /// then select first button of the left list.
        /// <para/>
        /// If there is no data in the left list,
        /// the first button of the right list will be selected.
        /// </summary>
        public void HandleListInteractable() => StartCoroutine(CoHandleListInteractable());
        public void StopHandleListInteractable() => StopCoroutine(CoHandleListInteractable());
        private IEnumerator CoHandleListInteractable()
        {
            yield return new WaitUntil(() =>
                (_gameScrollContent != null && _walletScrollContent != null)
                && (_gameScrollContent.childCount > 0 || _walletScrollContent.childCount > 0));

            switch (_gameScrollContent.childCount)
            {
                case > 0:
                    _walletListUi.SetInteractableAllButtons(false);
                    _gameListUi.SelectDefault();
                    break;
                case <= 0:
                    _walletListUi.SelectDefault();
                    break;
            }
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
            targetList.SelectDefault();
        }

        public void ConfirmedTransmission()
        {
            _gameListUi.UpdateList();
            _walletListUi.UpdateList();
        }
    }
}