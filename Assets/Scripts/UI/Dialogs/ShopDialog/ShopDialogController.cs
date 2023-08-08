using System;
using System.Collections;
using CryptoQuest.Events;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using CryptoQuest.UI.Dialogs;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.Shop
{
    public class ShopDialogController : AbstractDialogController<UIShopDialog>
    {
        private LocalizedString _localizedMessage;
        [SerializeField] private UIShopDialog _dialog;
        [SerializeField] private UIShopConfirmDialog _confirmDialog;

        [Header("Confirm Events")]
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _closeShopDialogEventChannel;
        [SerializeField] private VoidEventChannelSO _showShopConfirmDialogEventChannel;

        [Header("Raise Events")]
        [SerializeField] private LocalizedStringEventChannelSO _showShopDialogEventChannel;

        private void Start()
        {
            if (_dialog == null) return;
            _dialog.gameObject.SetActive(false);
            _yesNoDialogEventSO.Show(OnConfirmClicked, OnCancelClicked);
        }
        protected override void RegisterEvents()
        {
            _showShopConfirmDialogEventChannel.EventRaised += ShowConfirmDialog;
            _showShopDialogEventChannel.EventRaised += ShowDialog;
            _closeShopDialogEventChannel.EventRaised += CloseDialog;
        }

        protected override void UnregisterEvents()
        {
            _showShopConfirmDialogEventChannel.EventRaised -= ShowConfirmDialog;
            _showShopDialogEventChannel.EventRaised -= ShowDialog;
            _closeShopDialogEventChannel.EventRaised -= CloseDialog;
        }

        protected override void SetupDialog(UIShopDialog dialog)
        {
            if (_dialog == null)
            {
                _dialog = dialog;
            }
            StartCoroutine(CoSetupDialog());
        }

        private IEnumerator CoSetupDialog()
        {
            var handler = _localizedMessage.GetLocalizedStringAsync();
            yield return handler;
            if (handler.IsDone)
            {
                _dialog.SetDialogue(handler.Result)
                    .Show();
            }
        }

        private void ShowDialog(LocalizedString message)
        {
            _localizedMessage = message;
            if (_dialog != null)
            {
                _dialog.gameObject.SetActive(true);
                SetupDialog(_dialog);
                return;
            }
            LoadAssetDialog();
        }

        private void ShowConfirmDialog()
        {
            if (_confirmDialog != null)
            {
                _confirmDialog.gameObject.SetActive(true);
                return;
            }
        }

        private void CloseDialog()
        {
            _dialog.Close();
        }

        private void OnConfirmClicked()
        {
            Debug.Log("Confirmed!");
            //TODO: Confirm action
        }

        private void OnCancelClicked()
        {
            Debug.Log("Cancel");
            //TODO: Cancel action
        }
    }
}
