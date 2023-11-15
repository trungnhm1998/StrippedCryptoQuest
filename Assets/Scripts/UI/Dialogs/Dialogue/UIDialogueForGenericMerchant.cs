using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs.Dialogue
{
    public class UIDialogueForGenericMerchant : AbstractDialog
    {
        [Header("UI")]
        [SerializeField] private LocalizeStringEvent _messageUi;
        [SerializeField] private GameObject _arrowImg;

        private LocalizedString _message;

        public UIDialogueForGenericMerchant SetMessage(LocalizedString message)
        {
            _message = message;
            return this;
        }

        public UIDialogueForGenericMerchant SetArrow(bool active)
        {
            _arrowImg.SetActive(active);
            return this;
        }

        private void UpdateUIMessage() => _messageUi.StringReference = _message;

        public override void Show()
        {
            base.Show();
            UpdateUIMessage();
            _message = null;
        }
    }
}