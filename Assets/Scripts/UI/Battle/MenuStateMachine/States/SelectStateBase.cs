using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.UI.Battle.CommandsMenu;
using System.Collections.Generic;

namespace CryptoQuest.UI.Battle.MenuStateMachine.States
{
    public abstract class SelectStateBase : BattleMenuStateBase
    {
        protected readonly List<AbstractButtonInfo> _buttonInfos = new();

        public SelectStateBase(BattleMenuStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            SetupButtons();
        }

        protected void SetupButtons()
        {
            _buttonInfos.Clear();

            SetupButtonsInfo();

            _battlePanelController.OpenCommandDetailPanel(_buttonInfos);
        }

        protected abstract void SetupButtonsInfo();

        public override void OnExit()
        {
            _battlePanelController.CloseCommandDetailPanel();
            base.OnExit();
        }

        protected void HandleTarget()
        {
            var selectedAbility = _currentUnit.UnitLogic.SelectedAbility;
            var ability = selectedAbility.AbilitySO as AbilitySO;
            if (ability == null) return;

            var targetTypeSo = ability.SkillInfo.TargetType;
            var targetType = targetTypeSo.GetTargetType(_currentUnit, _battlePanelController);
            targetType.HandleTargets();
        }
    }
}