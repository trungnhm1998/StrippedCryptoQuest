using System;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone
{
    public class ConfirmStoneUpgradePresenter : MonoBehaviour
    {
        [field: SerializeField] public UIConfirmStoneUpgradePanel ConfirmStoneUpgradePanel { get; private set; }

        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }

        public event Action Confirmed;
        public event Action Canceled;

        private void OnEnable()
        {
            DialogPresenter.ConfirmYesEvent += HandleConfirmUpgrade;
            DialogPresenter.ConfirmNoEvent += HandleCancelUpgrade;
        }

        private void OnDisable()
        {
            DialogPresenter.ConfirmYesEvent -= HandleConfirmUpgrade;
            DialogPresenter.ConfirmNoEvent -= HandleCancelUpgrade;
        }

        private void HandleConfirmUpgrade()
        {
            Confirmed?.Invoke();
        }

        private void HandleCancelUpgrade()
        {
            Canceled?.Invoke();
        }
    }
}