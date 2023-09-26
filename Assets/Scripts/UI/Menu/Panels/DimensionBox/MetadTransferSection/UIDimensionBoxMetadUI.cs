using System;
using CryptoQuest.Events;
using CryptoQuest.Events.UI.Dialogs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public class UIDimensionBoxMetadUI : MonoBehaviour
    {
        public static event UnityAction<bool> ConfirmTransferEvent;
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;
        [SerializeField] private LocalizedString _transferMessageSuccess;
        [SerializeField] private LocalizedString _transferMessageFail;
        [SerializeField] private UIMetadSection _metadSection;
        [SerializeField] private GameObject _showConfirmDialog;
        [SerializeField] private GameObject _descriptionPanel;
        [SerializeField] private Text _ingameWalletMetad;
        [SerializeField] private Text _web3WalletMetad;
        [SerializeField] private LocalizeStringEvent _descriptionDialog;
        private bool _isTransfer;

        private void OnEnable()
        {
            _yesNoDialogEventSO.ShowEvent += ShowConfirmDialog;
            _yesNoDialogEventSO.HideEvent += HideConfirmDialog;
        }

        private void OnDisable()
        {
            _yesNoDialogEventSO.ShowEvent -= ShowConfirmDialog;
            _yesNoDialogEventSO.HideEvent -= HideConfirmDialog;
        }

        public void SetCurrentMetad(float ingameMetaD, float webMetaD)
        {
            _ingameWalletMetad.text = ingameMetaD.ToString();
            _web3WalletMetad.text = webMetaD.ToString();
        }

        public void ShowMessage(bool isSuccess)
        {
            _yesNoDialogEventSO.Show(OnConfirmClicked, OnCancelClicked);
            _descriptionDialog.StringReference = isSuccess ? _transferMessageSuccess : _transferMessageFail;
            _metadSection.DisableAllButtons();
            _isTransfer = isSuccess;
        }

        private void OnConfirmClicked()
        {
            _yesNoDialogEventSO.Hide();
            ConfirmTransferEvent?.Invoke(_isTransfer);
            _metadSection.Init();
        }

        private void OnCancelClicked()
        {
            _yesNoDialogEventSO.Hide();
            _metadSection.Init();
        }

        private void ShowConfirmDialog(Action yesAction, Action noAction)
        {
            SetActiveUI(true);
            _showConfirmDialog.transform.SetAsLastSibling();
        }

        private void HideConfirmDialog()
        {
            if (_showConfirmDialog.transform.childCount > 0)
            {
                Destroy(_showConfirmDialog.transform.GetChild(0).gameObject);
            }
            SetActiveUI(false);
        }

        private void SetActiveUI(bool isActive)
        {
            _descriptionPanel.SetActive(isActive);
            _showConfirmDialog.SetActive(isActive);
        }
    }
}
