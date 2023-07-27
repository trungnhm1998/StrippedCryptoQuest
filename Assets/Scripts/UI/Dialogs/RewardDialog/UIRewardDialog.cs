using System.Collections;
using CryptoQuest.Gameplay.Quest;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class UIRewardDialog : ModalWindow<UIRewardDialog>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _defaultSelectButton;
        [SerializeField] private LocalizeStringEvent _earnedEXP;
        [SerializeField] private LocalizeStringEvent _earnedGold;
        [SerializeField] private LocalizeStringEvent _earnedSouls;
        [SerializeField] private GameObject _rewardedItemPrefab;
        [SerializeField] private Transform _itemsContainerTransform;
        [SerializeField] private LocalizedString _noItemString;

        public UnityAction CloseButtonPressed;
        private RewardDialogData _rewardDialogData;

        protected override void OnBeforeShow() {
            DisplayItemsReward();
        }

        protected override void CheckIgnorableForClose() { }

        private void Awake()
        {
            StartCoroutine(CoSelectDefaultButton());
        }

        private IEnumerator CoSelectDefaultButton()
        {
            yield return new WaitForSeconds(.03f);
            _defaultSelectButton.Select();
        }

        public void OnCloseButtonPressed()
        {
            CloseButtonPressed.Invoke();
            Close();
        }

        public override UIRewardDialog Close()
        {
            gameObject.SetActive(false);
            return base.Close();
        }

        public UIRewardDialog SetDialogue(RewardDialogData rewardDialogData)
        {
            _rewardDialogData = rewardDialogData;
            return this;
        }

        private void DisplayItemsReward()
        {
            if (_rewardDialogData.ItemNames.Count <= 0)
            {
                SetRewardedItemLabel(_noItemString);
                return;
            }

            foreach (var itemName in _rewardDialogData.ItemNames)
            {
                SetRewardedItemLabel(itemName);
            }
        }

        private LocalizeStringEvent GetRewaredItemLabel()
        {
            var goItem = Instantiate(_rewardedItemPrefab, _itemsContainerTransform);

            var rewardedItemLabel = goItem.GetComponent<LocalizeStringEvent>();

            return rewardedItemLabel;
        }

        private void SetRewardedItemLabel(LocalizedString itemName)
        {
            var rewardedItemLabel = 
                GetRewaredItemLabel();

            rewardedItemLabel.StringReference = itemName;
        }
    }
}
