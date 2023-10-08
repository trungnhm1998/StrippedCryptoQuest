using System;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;

namespace CryptoQuest.Battle.States
{
    public class Intro : IState
    {
        private BattleStateMachine _battleStateMachine;

        private readonly UIGenericDialog _dialog;

        public Intro(UIGenericDialog dialog)
        {
            _dialog = dialog;
        }

        public void OnEnter(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
            var introUI = _battleStateMachine.IntroUI;
            _dialog
                .WithAutoHide(introUI.Duration)
                .WithHideCallback(() => _battleStateMachine.ChangeState(new SelectHeroesActions.SelectHeroesActions()))
                .WithMessage(introUI.IntroMessage)
                .Show();
            _battleStateMachine.BattleInput.EnableBattleInput();
        }

        public void OnExit(BattleStateMachine battleStateMachine)
        {
            try
            {
                GenericDialogController.Instance.Release(_dialog);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                throw;
            }
        }
    }
}