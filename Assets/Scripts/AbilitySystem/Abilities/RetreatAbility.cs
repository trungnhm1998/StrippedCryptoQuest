using System.Collections;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Executions;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class RetreatAbility : AbilityScriptableObject<RetreatAbilitySpec>
    {
        public UnityAction RetreatedEvent;
        public UnityAction RetreatFailedEvent;
    }

    public class RetreatAbilitySpec : GameplayAbilitySpec
    {
        private float _enemySpeed;

        public void Execute(float highestEnemyAgility)
        {
            _enemySpeed = highestEnemyAgility;
            TryActiveAbility();
        }

        public override bool CanActiveAbility()
        {
            Owner.AttributeSystem.TryGetAttributeValue(AttributeSets.Agility, out var agility);

            var rand = Random.Range(0f, 100f);
            var probabilityOfRetreat =
                BattleCalculator.CalculateProbabilityOfRetreat(_enemySpeed, agility.CurrentValue);
            var canActive = probabilityOfRetreat > 0 && rand <= probabilityOfRetreat;
            // TODO: Maybe implement template method pattern here
            if (!canActive)
                ((RetreatAbility)AbilitySO).RetreatFailedEvent?.Invoke();

            return base.CanActiveAbility() && canActive;
        }

        protected override IEnumerator OnAbilityActive()
        {
            ((RetreatAbility)AbilitySO).RetreatedEvent?.Invoke();
            yield break;
        }
    }
}