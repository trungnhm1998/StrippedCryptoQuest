using CryptoQuest.Events;
using CryptoQuest.UI.Dialogs;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace CryptoQuest.Shop.UI.Dialogs
{
    public class ShopDialogController : AbstractDialogController<UIShopDialog>
    {
        [SerializeField] private UIShopDialog _dialog;

        [Header("Raise Events")]
        [SerializeField] private LocalizedStringEventChannelSO _showShopDialogEventChannel;
        [SerializeField] private VoidEventChannelSO _hideDialogEventChannel;

        private void Start()
        {
            if (_dialog == null) return;
            _dialog.gameObject.SetActive(false);
        }

        protected override void RegisterEvents()
        {
            _showShopDialogEventChannel.EventRaised += ShowDialog;
            _hideDialogEventChannel.EventRaised += HideDialog;
        }

        protected override void UnregisterEvents()
        {
            _showShopDialogEventChannel.EventRaised -= ShowDialog;
            _hideDialogEventChannel.EventRaised -= HideDialog;
        }

        protected override void SetupDialog(UIShopDialog dialog)
        {
        }

        private void ShowDialog(LocalizedString message)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (_dialog != null)
            {
                _dialog.gameObject.SetActive(true);
                _dialog.SetDialogue(message).Show();
                return;
            }
            LoadAssetDialog();
        }

        private void HideDialog()
        {
            _dialog.gameObject.SetActive(false);
        }
    }
}