using System;
using System.Collections;
using CryptoQuest.Events;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using CryptoQuest.UI.Dialogs;
using CryptoQuest.UI.Dialogs.YesNoDialog;
using CryptoQuest.Shop.UI;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Shop.UI.Dialogs
{
    public class ShopDialogController : AbstractDialogController<UIShopDialog>
    {
        [SerializeField] private ShopInputManager shopInputManager;
        [SerializeField] private UIShopDialog _dialog;
        [SerializeField] private GameObject _yesNoDialog;

        [Header("Confirm Events")]
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;

        [Header("Raise Events")]
        [SerializeField] private LocalizedStringEventChannelSO _showShopDialogEventChannel;

        private void Start()
        {
            if (_dialog == null) return;
            _dialog.gameObject.SetActive(false);
        }

        protected override void RegisterEvents()
        {
            shopInputManager.BackEvent += HideConfirmDialog;
            shopInputManager.ExitEvent += HideConfirmDialog;

            _showShopDialogEventChannel.EventRaised += ShowDialog;

            _yesNoDialogEventSO.ShowEvent += ShowConfirmDialog;
            _yesNoDialogEventSO.HideEvent += HideConfirmDialog;
        }

        protected override void UnregisterEvents()
        {
            shopInputManager.BackEvent -= HideConfirmDialog;
            shopInputManager.ExitEvent -= HideConfirmDialog;

            _showShopDialogEventChannel.EventRaised -= ShowDialog;
            _yesNoDialogEventSO.ShowEvent -= ShowConfirmDialog;
            _yesNoDialogEventSO.HideEvent -= HideConfirmDialog;
        }

        protected override void SetupDialog(UIShopDialog dialog)
        {
        }

        private void ShowDialog(LocalizedString message)
        {
            if (_dialog != null)
            {
                _dialog.gameObject.SetActive(true);
                _dialog.SetDialogue(message).Show();
                return;
            }
            LoadAssetDialog();
        }

        private void ShowConfirmDialog(Action yesAction, Action noAction )
        {
            if (_yesNoDialog != null)
            {
                _yesNoDialog.transform.SetAsLastSibling();
                _yesNoDialog.SetActive(true);
            }
        }

        private void HideConfirmDialog()
        {
            if (_yesNoDialog != null)
            {
                //Manual destroy due to it auto Instantiate new object when showing and not destroy after hiding
                if (_yesNoDialog.transform.childCount > 0)
                {
                    Destroy(_yesNoDialog.transform.GetChild(0).gameObject);
                }    
                _yesNoDialog.SetActive(false);
            }
        }
    }
}
