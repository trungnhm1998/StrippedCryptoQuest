using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using System.Collections;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class BattleDialogController : AbstractDialogController<UIBattleDialog>
    {
        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _endActionPhaseEventChannel;
        [SerializeField] private LocalizedStringEventChannelSO _showBattleDialogEventChannel;

        private LocalizedString _localizedMessage;
        [SerializeField] private UIBattleDialog _dialog;

        protected override void RegisterEvents()
        {
            _showBattleDialogEventChannel.EventRaised += ShowDialog;
            _endActionPhaseEventChannel.EventRaised += CloseDialog;
        }

        protected override void UnregisterEvents()
        {
            _showBattleDialogEventChannel.EventRaised -= ShowDialog;
            _endActionPhaseEventChannel.EventRaised -= CloseDialog;
        }

        private void Start()
        {
            if (_dialog == null) return;
            _dialog.gameObject.SetActive(false);
        }

        protected override void SetupDialog(UIBattleDialog dialog)
        {
            dialog.SetDialogue(_localizedMessage.GetLocalizedString())
                .Show();
            if (_dialog != null) return;
            _dialog = dialog;
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

        private void CloseDialog()
        {
            _dialog.Close();
        }
    }
}