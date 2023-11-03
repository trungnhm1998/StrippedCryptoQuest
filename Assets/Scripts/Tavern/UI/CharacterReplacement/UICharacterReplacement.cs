using UnityEngine;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class UICharacterReplacement : UIAbstractTavern
    {
        [SerializeField] private UICharacterList _gameListUi;
        [SerializeField] private UICharacterList _walletListUi;

        [SerializeField] private Transform _gameScrollContent;
        [SerializeField] private Transform _walletScrollContent;

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
            var currentList = currentItem.Parent;
            var otherList = currentList == _gameScrollContent ? _walletScrollContent : _gameScrollContent;
            currentItem.Transfer(otherList);

            _gameListUi.SetInteractableAllButtons(otherList == _gameScrollContent);
            _walletListUi.SetInteractableAllButtons(otherList == _walletScrollContent);
        }

        public void SwitchList(Vector2 direction)
        {
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