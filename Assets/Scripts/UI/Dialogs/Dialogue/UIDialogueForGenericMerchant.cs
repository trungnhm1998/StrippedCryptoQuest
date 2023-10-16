using CryptoQuest.Gameplay.Quest.Dialogue;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs.Dialogue
{
    public class UIDialogueForGenericMerchant : AbstractDialog
    {
        [Header("UI")]
        [SerializeField] private LocalizeStringEvent _messageUi;

        private LocalizedString _message;

        public UIDialogueForGenericMerchant SetMessage(LocalizedString message)
        {
            _message = message;
            UpdateUIMessage();
            return this;
        }

        private void UpdateUIMessage()
        {
            _messageUi.StringReference = _message;
        }
    }
}