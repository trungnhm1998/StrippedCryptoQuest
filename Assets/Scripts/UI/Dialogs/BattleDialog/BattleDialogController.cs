using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class BattleDialogController : AbstractDialogController<UIBattleDialog>
    {
        [Header("Listen Events")]
        [SerializeField] private BattleActionDataEventChannelSO _gotActionDataEventChannel;
        [SerializeField] private VoidEventChannelSO _doneActionEventChannel;

        [Header("Raise Events")]
        [SerializeField] private StringEventChannelSO _showBattleDialogEventChannel;

        private string _message;
        [SerializeField] private UIBattleDialog _dialog;

        protected override void RegisterEvents()
        {
            _showBattleDialogEventChannel.EventRaised += ShowDialog;
            _doneActionEventChannel.EventRaised += OnUnitDoneAction;
            _gotActionDataEventChannel.EventRaised += OnGotActionData;
        }

        protected override void UnregisterEvents()
        {
            _showBattleDialogEventChannel.EventRaised -= ShowDialog;
            _doneActionEventChannel.EventRaised -= OnUnitDoneAction;
            _gotActionDataEventChannel.EventRaised -= OnGotActionData;
        }

        private void Start()
        {
            if (_dialog == null) return;
            _dialog.gameObject.SetActive(false);
        }

        protected override void SetupDialog(UIBattleDialog dialog)
        {
            if (_dialog == null)
            {
                _dialog = dialog;
            }
            _dialog.SetDialogue(_message)
                .Show();
        }

        private void ShowDialog(string dialogue)
        {
            _message = dialogue;
            if (_dialog != null)
            {
                _dialog.SetDialogue(_message)
                    .Show();
                return;
            }
            LoadAssetDialog();
        }

        private void OnGotActionData(BattleActionDataSO data)
        {
            _dialog.gameObject.SetActive(true);
            _dialog.SetDialogue(data.Log.GetLocalizedString())
                .Show();
        }

        private void OnUnitDoneAction()
        {
        }

        private void OnEndActionPhase()
        {
        }
    }
}