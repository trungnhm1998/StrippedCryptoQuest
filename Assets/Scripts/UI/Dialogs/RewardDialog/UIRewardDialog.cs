using CryptoQuest.Battle.Actions;
using CryptoQuest.Battle.Audio;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using CryptoQuest.Quest.Controllers;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class UIRewardDialog : AbstractDialog
    {
        [Header("Child Components"), SerializeField]
        private InputMediatorSO _inputMediator;

        [SerializeField] private GameStateSO _gameStateSo;

        [field: Header("Transform"), SerializeField]
        public Transform TopContainer { get; private set; }

        [field: SerializeField] public Transform BottomContainer { get; private set; }

        [Header("UI"), SerializeField] private GameObject _topNone;

        [SerializeField] private GameObject _bottomNone;

        [field: Header("Components"), SerializeField]
        private Button _defaultSelectButton;

        [field: SerializeField] public UIRewardItem RewardItemPrefab { get; private set; }
        [field: SerializeField] private RewardScroll _scroll;
        [field: SerializeField] private InputAction _inputAction;

        private RewardDialogData _rewardDialogData;

        public void OnCloseButtonPressed() => Hide();

        private void OnCloseImmediately(InputAction.CallbackContext action)
        {
            if (!action.performed) return;
            Hide();
        }

        public override void Show()
        {
            base.Show();

            ActionDispatcher.Dispatch(new PauseCutsceneAction());
            _inputAction.Enable();
            _inputAction.performed += OnCloseImmediately;

            _inputMediator.MenuConfirmedEvent += OnCloseButtonPressed;

            _defaultSelectButton.Select();

            if (!_rewardDialogData.IsValid()) return;
            _inputMediator.DisableAllInput();

            DisplayItemsReward();
        }

        public override void Hide()
        {
            base.Hide();

            ActionDispatcher.Dispatch(new PlayCachedBgmAction());
            ActionDispatcher.Dispatch(new ResumeCutsceneAction());
            // TODO: CHECK CUTSCENE BEFORE SHOW LEVEL UP
            ActionDispatcher.Dispatch(new ShowLevelUpAction());

            _inputAction.Disable();
            _inputAction.performed -= OnCloseImmediately;

            _inputMediator.MenuConfirmedEvent -= OnCloseButtonPressed;

            if (_gameStateSo.CurrentGameState != EGameState.Field) return;
            _inputMediator.EnableMapGameplayInput();

            CleanUpRewardView();
        }

        public UIRewardDialog SetReward(RewardDialogData rewardDialogData)
        {
            _rewardDialogData = rewardDialogData;
            return this;
        }

        private void DisplayItemsReward()
        {
            CleanUpRewardView();

            foreach (var loot in _rewardDialogData.Loots)
            {
                var uiReward = Instantiate(RewardItemPrefab, loot.IsItem ? BottomContainer : TopContainer);
                uiReward.SetLoot(loot);
            }

            _topNone.SetActive(TopContainer.childCount <= 0);
            _bottomNone.SetActive(BottomContainer.childCount <= 0);

            _scroll.UpdateStep();
        }

        private void CleanUpRewardView()
        {
            foreach (Transform child in TopContainer) Destroy(child.gameObject);
            foreach (Transform child in BottomContainer) Destroy(child.gameObject);
        }
    }
}