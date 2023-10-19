using System.Collections;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class RetreatAbility : AbilityScriptableObject
    {
        protected override GameplayAbilitySpec CreateAbility() => new RetreatAbilitySpec(this);
    }

    public class RetreatAbilitySpec : GameplayAbilitySpec
    {
        private float _enemySpeed;
        private RetreatAbility _retreatAbility;

        public bool CanRetreatBattle { get; set; } = true;

        public RetreatAbilitySpec(RetreatAbility retreatAbility)
        {
            _retreatAbility = retreatAbility;
        }

        public void Execute(float highestEnemyAgility)
        {
            _enemySpeed = highestEnemyAgility;
            TryActiveAbility();
        }

        protected override IEnumerator OnAbilityActive()
        {
            Owner.AttributeSystem.TryGetAttributeValue(AttributeSets.Agility, out var agility);
            var character = Owner.GetComponent<Battle.Components.Character>();

            var rand = Random.Range(0f, 100f);
            var probabilityOfRetreat =
                BattleCalculator.CalculateProbabilityOfRetreat(_enemySpeed, agility.CurrentValue);
            var canActive = probabilityOfRetreat > 0 && rand <= probabilityOfRetreat && CanRetreatBattle;
            if (!canActive)
            {
                BattleEventBus.RaiseEvent(new RetreatFailedEvent { Character = character });
                EndAbility();
                yield break;
            }

            BattleEventBus.RaiseEvent(new RetreatSucceedEvent { Character = character });
            EndAbility();
        }
    }
}