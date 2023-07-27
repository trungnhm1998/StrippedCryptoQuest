using UnityEngine;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class PagingDialogController : AbstractDialogController<UIPagingDialog>
    {
        [Header("Listen Events")]
        [SerializeField] private PagingDialogEventChannelSO _showPagingDialogEventChannel;

        private PagingDialog _dialogue;

        protected override void RegisterEvents()
        {
            _showPagingDialogEventChannel.EventRaised += ShowDialog;
        }

        protected override void UnregisterEvents()
        {
            _showPagingDialogEventChannel.EventRaised -= ShowDialog;
        }

        private void ShowDialog(PagingDialog dialogue)
        {
            _dialogue = dialogue;
            LoadAssetDialog();
        }

        protected override void SetupDialog(UIPagingDialog dialog)
        {
            dialog.SetDialogue(_dialogue)
                .Show();
        }
    }
}
