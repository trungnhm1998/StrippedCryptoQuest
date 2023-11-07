using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using CharacterBehaviour = CryptoQuest.Battle.Components.Character;
using UnityEngine;
using CryptoQuest.Battle.Components;
using System.Collections;

namespace CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Condition Skill/Multiple Attack",
        fileName = "ConditionSkill")]
    public class MultipleAttacksPassive : PostNormalAttackPassiveBase
    {
        [field: SerializeField] public float AdditionalAttacks { get; private set; } = 1;
        protected override GameplayAbilitySpec CreateAbility()
            => new MultipleAttackPassiveSpec(this);
#if UNITY_EDITOR
        public override PostNormalAttackPassiveBase CreateInstance()
        {
            return (MultipleAttacksPassive)Activator.CreateInstance(this.GetType());
        }
#endif
    }

    public class MultipleAttackPassiveSpec : PostNormalAttackPassiveSpecBase
    {
        private MultipleAttacksPassive _ability;
        private bool _isActivatedInThisTurn;

        public MultipleAttackPassiveSpec(MultipleAttacksPassive ability)
        {
            _ability = ability;
        }

        protected override IEnumerator OnAbilityActive()
        {
            yield return base.OnAbilityActive();
            Character.TurnStarted += SetFlagActivated;
            yield break;
        }

        protected override void OnAbilityEnded()
        {
            base.OnAbilityEnded();
            Character.TurnStarted -= SetFlagActivated;
        }

        private void SetFlagActivated()
        {
            _isActivatedInThisTurn = false;
        }

        protected override void OnAttacked(DamageContext postAttackContext)
        {
            // I have to check flag because when normal attack again this method
            // will be call infinitely
            if (_isActivatedInThisTurn) return;
            _isActivatedInThisTurn = true;

            if (!IsTargetValid(Character)) return;
            Character.TryGetComponent<CommandExecutor>(out var commandExecutor);
            for (int i = 0; i < _ability.AdditionalAttacks; i++)
            {
                commandExecutor.Command.Execute();
            }
        }
    }
}