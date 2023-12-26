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

        public List<int> SelectedGameItemsIds { get; } = new();
        public List<int> SelectedDboxItemsIds { get; } = new();

        public void Transfer(UITavernItem currentItem)
        {
            var currentList = currentItem.Parent;
            if (currentList == _gameScrollContent) SelectedGameItemsIds.Add(currentItem.Id);
            else SelectedDboxItemsIds.Add(currentItem.Id);

            _gameListUi.SetInteractableAllButtons(currentList == _gameScrollContent);
            _walletListUi.SetInteractableAllButtons(!(currentList == _gameScrollContent));

            Debug.Log($"<color=white>game={SelectedGameItemsIds.Count} -- wallet={SelectedDboxItemsIds.Count}</color>");
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
                    FocusList(_gameListUi);
                    break;
                case <= 0:
                    FocusList(_walletListUi);
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