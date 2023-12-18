using System.Collections;
using CryptoQuest.Events.UI.Dialogs;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class RewardDialogController : MonoBehaviour
    {
        [SerializeField] private RewardDialogEventChannelSO _rewardDialogEvent;
        private UIRewardDialog _dialog;

        public void OnEnable() => _rewardDialogEvent.ShowEvent += ShowDialog;

        public void OnDisable() => _rewardDialogEvent.ShowEvent -= ShowDialog;

        private void OnDestroy() => GenericRewardButtonDialogController.Instance.Release(_dialog);

        private void ShowDialog(RewardDialogData rewardDialogData) =>
            StartCoroutine(CoLoadDialogAndShowReward(rewardDialogData));

        private IEnumerator CoLoadDialogAndShowReward(RewardDialogData rewardDialogData)
        {
            if (_dialog == null)
            {
                yield return
                    GenericRewardButtonDialogController.Instance.CoInstantiate(dialog => _dialog = dialog);
            }

            _dialog
                .SetReward(rewardDialogData)
                .Show();
        }
    }
}