using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;

namespace CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands
{
    public class UseAbilityCommand : LogBeforeVFXCommand
    {
        // private IBattleUnit _battleUnit;
        // private SimpleGameplayAbilitySpec _abilitySpec;
        //
        // public UseAbilityCommand(IBattleUnit battleUnit, SimpleGameplayAbilitySpec abilitySpec)
        //     : base(abilitySpec.AbilityDef.ActionData)
        // {
        //     _battleUnit = battleUnit;
        //     _abilitySpec = abilitySpec;
        // }

        public override void Execute()
        {
            // TODO: REFACTOR BATTLE
            // var targets = _battleUnit.UnitLogic.TargetContainer;
            // foreach (var target in targets)
            // {
            //     _commandData.Init(target);
            //     _abilitySpec.Active(target);
            // }
            //
            // _commandData.AddStringVar("unitName", _battleUnit.UnitInfo.DisplayName);
            base.Execute();
        }

        public UseAbilityCommand(BattleActionDataSO data) : base(data) { }
    }
}