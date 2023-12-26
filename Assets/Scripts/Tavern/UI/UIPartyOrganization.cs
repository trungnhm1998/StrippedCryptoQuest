using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public class UIPartyOrganization : UIAbstractTavern
    {
        [SerializeField] private UICharacterList _partyUi;
        [SerializeField] private UICharacterList _gameListUi;

        [SerializeField] private Transform _partyScrollContent;
        [SerializeField] private Transform _gameScrollContent;

        public List<int> SelectedNonPartyCharacterIds { get; private set; } = new();
        public List<int> SelectedPartyCharacterIds { get; private set; } = new();

        public void Transfer(UITavernItem currentItem)
        {
            var currentList = currentItem.Parent;
            if (currentList == _gameScrollContent) SelectedNonPartyCharacterIds.Add(currentItem.Id);
            else SelectedPartyCharacterIds.Add(currentItem.Id);
        }

        /// <summary>
        /// This method will disable all the buttons of the right list if the left list has data,
        /// then select first button of the left list.
        /// <para/>
        /// If there is no data in the left list,
        /// the first button of the right list will be selected.
        /// </summary>
        public void HandleListInteractable() => StartCoroutine(CoHandleListInteractable());
        private IEnumerator CoHandleListInteractable()
        {
            yield return new WaitUntil(() =>
                (_partyScrollContent != null && _gameScrollContent != null)
                && (_partyScrollContent.childCount > 0 || _gameScrollContent.childCount > 0));

            switch (_partyScrollContent.childCount)
            {
                case > 0:
                    _gameListUi.SetInteractableAllButtons(false);
                    _partyUi.SelectDefault();
                    break;
                case <= 0:
                    _gameListUi.SelectDefault();
                    break;
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
            targetList.SelectDefault();
        }

        public void ConfirmedTransmission()
        {
            _partyUi.UpdateList();
            _gameListUi.UpdateList();
        }

        private void OnDestroy() => StopCoroutine(CoHandleListInteractable());
    }
}