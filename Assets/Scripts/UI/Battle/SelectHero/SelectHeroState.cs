using UnityEngine;
using CryptoQuest.UI.Battle.StateMachine;
using System;

namespace CryptoQuest.UI.Battle.SelectHero
{
    public class SelectHeroState : BattleMenuStateBase
    {
        public static event Action EnteredState;

        private UISelectHeroButton _selectHeroButton;

        public SelectHeroState(BattleMenuStateMachine stateMachine, UISelectHeroButton button)
            : base(stateMachine)
        {
            _selectHeroButton = button;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            EnteredState?.Invoke();
            _selectHeroButton.SetUIActive(true);
            _selectHeroButton.SelectButton();
        }

        public override void OnExit()
        {
            base.OnExit();
            _selectHeroButton.SetUIActive(false);
        }
    }
}