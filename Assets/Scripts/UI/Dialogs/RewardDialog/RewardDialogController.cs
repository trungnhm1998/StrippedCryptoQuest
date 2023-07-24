using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Gameplay.Quest;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs
{
    public class RewardDialogController : AbstractDialogController<UIRewardDialog>
    {
        [SerializeField] private RewardDialogEventChannelSO _rewardDialogEvent;
        private RewardDialogData _rewardDialogData;

        protected override void RegisterEvents()
        {
            _rewardDialogEvent.ShowEvent += ShowDialog;
        }

        protected override void UnregisterEvents()
        {
            _rewardDialogEvent.ShowEvent -= ShowDialog;
        }

        private void ShowDialog(RewardDialogData rewardDialogData)
        {
            _rewardDialogData = rewardDialogData;
            LoadAssetDialog();
        }

        protected override void SetupDialog(UIRewardDialog dialog)
        {
            dialog
                .SetDialogue(_rewardDialogData)
                .Show();
        }
    }
}