using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class UICharacterReplacement : UIAbstractTavern
    {
        [SerializeField] private Transform _gameScrollContent;
        [SerializeField] private Transform _walletScrollContent;

        public override void EnterTransferSection()
        {
            base.EnterTransferSection();
        }

        public override void ExitTransferSection()
        {
            base.ExitTransferSection();
        }

        public override void ResetTransfer()
        {
        }

        public override void SendItems()
        {
            SetInteractableAllButtons(_gameScrollContent, false);
            SetInteractableAllButtons(_walletScrollContent, false);
        }

        public void OnInspectItem()
        {
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