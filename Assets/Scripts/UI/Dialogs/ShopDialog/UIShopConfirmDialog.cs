using System;
using CryptoQuest.Events.UI.Dialogs;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs.Shop
{
    public class UIShopConfirmDialog : MonoBehaviour
    {
        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _showConfirmDialogEventChannel;
        [SerializeField] private VoidEventChannelSO _closeConfirmDialogEventChannel;

        private void OnEnable()
        {
            _showConfirmDialogEventChannel.EventRaised += ShowDialog;
            _closeConfirmDialogEventChannel.EventRaised += CloseDialog;
        }

        private void OnDisable()
        {
            _showConfirmDialogEventChannel.EventRaised -= ShowDialog;
            _closeConfirmDialogEventChannel.EventRaised -= CloseDialog;
        }

        private void ShowDialog()
        {
            gameObject.SetActive(true);
        }

        private void CloseDialog()
        {
            gameObject.SetActive(false);
        }
    }
}