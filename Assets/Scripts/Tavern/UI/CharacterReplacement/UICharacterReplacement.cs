using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class UICharacterReplacement : UIAbstractTavern
    {
        [SerializeField] private Transform _gameScrollContent;
        [SerializeField] private Transform _walletScrollContent;

        public void StateEntered()
        {
            SetInteractableAllButtons(_walletScrollContent, false);

            UITavernItem.Pressed += Transfer;
        }

        public void StateExited()
        {
            SetInteractableAllButtons(_gameScrollContent, false);
            SetInteractableAllButtons(_walletScrollContent, false);

            UITavernItem.Pressed -= Transfer;
        }

        private void Transfer(UITavernItem currentItem)
        {
            Transform itemNewParent;
            var isGameBoardAsCurrentParent = currentItem.Parent == _gameScrollContent;

            if (isGameBoardAsCurrentParent)
                itemNewParent = _walletScrollContent;
            else
                itemNewParent = _gameScrollContent;

            currentItem.Transfer(itemNewParent);

            SetInteractableAllButtons(_gameScrollContent, !isGameBoardAsCurrentParent);
            SetInteractableAllButtons(_walletScrollContent, isGameBoardAsCurrentParent);
        }

        public override void ResetTransfer()
        {
        }

        public override void SendItems()
        {
            SetInteractableAllButtons(_gameScrollContent, false);
            SetInteractableAllButtons(_walletScrollContent, false);
        }

        private void SetInteractableAllButtons(Transform boardList, bool isEnabled)
        {
            foreach (Transform item in boardList)
            {
                item.GetComponent<Button>().enabled = isEnabled;
            }
        }

    }
}