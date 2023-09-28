using System.Collections;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class UIRewardDialog : ModalWindow<UIRewardDialog>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [SerializeField] private float _autoCloseDelay = 1.5f;
        [SerializeField] private Button _defaultSelectButton;
        [SerializeField] private GameObject _topNone;
        [SerializeField] private GameObject _bottomNone;
        [field: SerializeField] public UIRewardItem RewardItemPrefab { get; private set; }
        [field: SerializeField] public Transform TopContainer { get; private set; }
        [field: SerializeField] public Transform BottomContainer { get; private set; }
        [SerializeField] private RewardScroll _scroll;

        public UnityAction CloseButtonPressed;
        private RewardDialogData _rewardDialogData;

        protected override void OnBeforeShow()
        {
            if (_rewardDialogData.IsValid() == false) return;
            _inputMediator.DisableAllInput();
            DisplayItemsReward();
        }

        protected override void CheckIgnorableForClose() { }

        private IEnumerator Start()
        {
            yield return null;
            _defaultSelectButton.Select();
        }

        public void OnCloseButtonPressed()
        {
            CloseButtonPressed?.Invoke();
            Close();
        }

        public override UIRewardDialog Close()
        {
            gameObject.SetActive(false);
            _inputMediator.EnableMapGameplayInput();
            return base.Close();
        }

        public UIRewardDialog SetDialogue(RewardDialogData rewardDialogData)
        {
            _rewardDialogData = rewardDialogData;
            return this;
        }

        private void DisplayItemsReward()
        {
            foreach (var reward in _rewardDialogData.RewardsInfos)
                reward.CreateUI(this);

            _topNone.SetActive(!(TopContainer.childCount > 0));
            _bottomNone.SetActive(!(BottomContainer.childCount > 0));
            if (_topNone.activeSelf && _bottomNone.activeSelf) StartCoroutine(CoAutoClose());
            _scroll.UpdateStep();
        }

        private IEnumerator CoAutoClose()
        {
            yield return new WaitForSeconds(_autoCloseDelay);
            Close();
        }

        public UIRewardItem InstantiateReward(Transform parent) => Instantiate(RewardItemPrefab, parent);
    }
}