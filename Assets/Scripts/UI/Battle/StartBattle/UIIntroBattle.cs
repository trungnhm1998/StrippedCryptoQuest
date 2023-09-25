using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.PushdownFSM;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Battle.StartBattle
{
    public class UIIntroBattle : MonoBehaviour
    {
        [SerializeField] private BattleManager _battleManager;
        [SerializeField] private BattleStateMachine _controller;
        [SerializeField] private GameObject _inBattleUI;
        [SerializeField] private LocalizedString _battleAppearPrompt;
        [SerializeField] private float _duration = 3f;

        private void OnEnable()
        {
            _battleManager.Initialized += StartBattle;
        }

        private void OnDisable()
        {
            _battleManager.Initialized -= StartBattle;
        }

        private void StartBattle()
        {
            _controller.GoToState(new IntroState(this));
        }

        private class IntroState : IState
        {
            private readonly UIIntroBattle _introBattleUI;
            private UIGenericDialog _dialog;

            public IntroState(UIIntroBattle introBattleUI)
            {
                _introBattleUI = introBattleUI;
            }

            public void OnEnter()
            {
                _introBattleUI._inBattleUI.SetActive(false);
                GenericDialogController.Instance.CreateDialog(PromptCreated);
            }

            private void PromptCreated(UIGenericDialog dialog)
            {
                _dialog = dialog;
                _dialog
                    .WithAutoHide(_introBattleUI._duration)
                    .WithHideCallback(GoToStartState)
                    .WithMessage(_introBattleUI._battleAppearPrompt)
                    .Show();
            }

            private void GoToStartState()
            {
                _introBattleUI._controller.GoToPreviousState();
                _introBattleUI._inBattleUI.SetActive(true);
            }

            public void OnExit() { }
        }
    }
}