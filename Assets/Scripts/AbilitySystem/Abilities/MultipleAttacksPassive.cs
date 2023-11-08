using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    /// <summary>
    /// Apply an instant effect to owner when the action is done
    /// </summary>
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Multiple attack passive",
        fileName = "MultipleAttacksPassive")]
    public class MultipleAttacksPassive : PassiveAbility
    {
        [field: SerializeField] public int Attacks { get; private set; } = 1;

        protected override GameplayAbilitySpec CreateAbility()
            => new MultipleAttacksPassiveSpec();
    }

    public class MultipleAttacksPassiveSpec : PassiveAbilitySpec
    {
        private CommandExecutor _commandExecutor;
        private MultipleAttacksPassive _def;
        private TinyMessageSubscriptionToken _eventToken;

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);
            Character.TryGetComponent(out _commandExecutor);
            _def = (MultipleAttacksPassive)abilitySO;
        }

        protected override IEnumerator OnAbilityActive()
        {
            _commandExecutor.PreExecuteCommand += WaitUntilRepeatCommandExecuted;
            yield break;
        }

        protected override void OnAbilityEnded() =>
            _commandExecutor.PreExecuteCommand -= WaitUntilRepeatCommandExecuted;

        private void WaitUntilRepeatCommandExecuted() =>
            _eventToken = BattleEventBus.SubscribeEvent<RepeatableCommandExecutedEvent>(RepeatCommand);

        private void RepeatCommand(RepeatableCommandExecutedEvent ctx)
        {
            BattleEventBus.UnsubscribeEvent(_eventToken);
            // just to make sure, even though we only listen to the event while the owner executing their command
            if (ctx.Character != Character) return;
            _commandExecutor.Command.Execute();
        }
    }
}