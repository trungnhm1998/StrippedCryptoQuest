using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs.PagingDialog
{
    public class PagingDialogController : AbstractDialogController<UIPagingDialog>
    {
        [Header("Listen Events")]
        [SerializeField] private PagingDialogEventChannelSO _showPagingDialogEventChannel;

        private Gameplay.Battle.Core.ScriptableObjects.Events.PagingDialog _dialogue;

        protected override void RegisterEvents()
        {
            _showPagingDialogEventChannel.EventRaised += ShowDialog;
        }

        protected override void UnregisterEvents()
        {
            _showPagingDialogEventChannel.EventRaised -= ShowDialog;
        }

        private void ShowDialog(Gameplay.Battle.Core.ScriptableObjects.Events.PagingDialog dialogue)
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
