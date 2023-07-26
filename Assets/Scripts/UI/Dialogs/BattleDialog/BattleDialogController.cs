using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class BattleDialogController : AbstractDialogController<UIBattleDialog>
    {
        [Header("Listen Events")]
        [SerializeField] private BattleActionDataEventChannelSO _gotActionDataEventChannel;
        [SerializeField] private VoidEventChannelSO _doneActionEventChannel;
        [SerializeField] private VoidEventChannelSO _endActionPhaseEventChannel;

        [Header("Raise Events")]
        [SerializeField] private LocalizedStringEventChannelSO _showBattleDialogEventChannel;
        [SerializeField] private VoidEventChannelSO _showNextMarkEventChannel;

        private LocalizedString _localizedMessage;
        [SerializeField] private UIBattleDialog _dialog;

        protected override void RegisterEvents()
        {
            _showBattleDialogEventChannel.EventRaised += ShowDialog;
            _doneActionEventChannel.EventRaised += OnUnitDoneAction;
            _gotActionDataEventChannel.EventRaised += OnGotActionData;
            _endActionPhaseEventChannel.EventRaised += CloseDialog;
        }

        protected override void UnregisterEvents()
        {
            _showBattleDialogEventChannel.EventRaised -= ShowDialog;
            _doneActionEventChannel.EventRaised -= OnUnitDoneAction;
            _gotActionDataEventChannel.EventRaised -= OnGotActionData;
            _endActionPhaseEventChannel.EventRaised += CloseDialog;
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
            _dialog.gameObject.SetActive(true);
            _localizedMessage = message;
            if (_dialog != null)
            {
                SetupDialog(_dialog);
                return;
            }
            LoadAssetDialog();
        }

        private void CloseDialog()
        {
            _dialog.Close();
        }

        private void OnGotActionData(BattleActionDataSO data)
        {
            ShowDialog(data.Log);
        }

        private void OnUnitDoneAction()
        {
            _showNextMarkEventChannel.RaiseEvent();
        }
    }
}