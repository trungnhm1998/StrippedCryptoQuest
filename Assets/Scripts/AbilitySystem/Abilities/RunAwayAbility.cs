using System.Collections;
using CryptoQuest.Battle;
using CryptoQuest.Battle.Components.EnemyComponents;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI.Logs;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class RunAwayAbility : CastSkillAbility
    {
        protected override GameplayAbilitySpec CreateAbility() =>
            new RunAwayAbilitySpec(this);
    }

    public class RunAwayAbilitySpec : CastSkillAbilitySpec
    {
        private readonly RunAwayAbility _def;

        public RunAwayAbilitySpec(RunAwayAbility def) : base(def)
        {
            _def = def;
        }

        protected override void InternalExecute(AbilitySystemBehaviour target)
        {
            EnemySpecialAbilityController.OnEnemyFled?.Invoke(Owner);
        }

        protected override IEnumerator OnAbilityActive()
        {
            if (!IsRunAwaySuccess()) yield break;

            RegisterBattleEndedEvents();

            ExecuteAbility();

            EndAbility();
        }

        private bool IsRunAwaySuccess()
        {
            var roll = Random.Range(0, 100);
            var result = roll < _def.SuccessRate;
            var resultMessage = result ? "Success" : "Failed";
            Debug.Log($"Casting {_def.name} with success rate {_def.SuccessRate} and roll {roll}: {resultMessage}");
            var character = Owner.GetComponent<Battle.Components.Character>();

            if (result)
            {
                var deadCommand = new EnemyDeadCommand(Owner.GetComponent<EnemyDeadBehaviour>());
                BattleEventBus.RaiseEvent(new EnqueuePresentCommandEvent(deadCommand));
            }

            BattleEventBus.RaiseEvent(new FledEvent(character, result));

            return result;
        }
    }
}