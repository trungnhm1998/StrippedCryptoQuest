using System;
using System.Collections;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CryptoQuest.Character.Ability
{
    /// <summary>
    /// https://docs.google.com/spreadsheets/d/1EDrXrT1if63TLam1km_dLx2VvHt2rks2OWXEUq5OEPg/edit#gid=2122798992
    /// Base data class for character skill
    /// </summary>
    public class CastableAbility : AbilityScriptableObject
    {
        [field: SerializeField] public GameplayEffectDefinition Effect { get; private set; }

        /// <summary>
        /// The effect that cost to cast this ability
        /// The <see cref="GameplayEffectSpec.Source"/> is the caster
        /// </summary>
        public GameplayEffectDefinition Cost;

        private SkillInfo _info;
        public SkillInfo Info => _info;

        protected override GameplayAbilitySpec CreateAbility()
        {
            var effectInstance = Instantiate(Effect);
            var ability = new CastableAbilitySpec
            {
                Effect = effectInstance
            };
            return ability;
        }

        public void InitAbilityInfo(SkillInfo info)
        {
            _info = info;
        }

        public void InitAbilityEffect(GameplayEffectDefinition configuredEffect)
        {
            // TODO: REFACTOR GAS
            // if (configuredEffect is CryptoQuestGameplayEffect effectInstance)
            // {
            //     Effect = effectInstance;
            // }
        }
    }

    public class CastableAbilitySpec : GameplayAbilitySpec
    {
        public event Action NotEnoughResourcesToCast;
        public GameplayEffectDefinition Effect { get; set; }
        private CharacterBehaviourBase _target;
        private AbilitySystemBehaviour _targetSystem;
        private CastableAbility SkillDef => (CastableAbility)AbilitySO;

        private GameplayEffectDefinition _costEffect;

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);

            if (SkillDef.Cost == null) return;
            _costEffect = Object.Instantiate(SkillDef.Cost);
            _costEffect.EffectDetails.Modifiers[0].Value = -SkillDef.Info.Cost; // I think this is a bad code
        }

        public override bool CanActiveAbility()
        {
            return base.CanActiveAbility() && CheckCost();
        }

        /// <summary>
        /// GameplayAbility.cpp::CheckCost line 906
        /// Create a cost effect spec, use the modifier and calculate the remaining resources, if the remaining resources
        /// is greater than 0 apply the effect
        /// </summary>
        /// <returns>Return true if after subtracted attribute that the cost needs greater than 0</returns>
        public bool CheckCost()
        {
            // TODO: REFACTOR GAS
            // if (_costEffect == null) return true;
            // if (Owner == null) return true;
            //
            // if (Owner.CanApplyAttributeModifiers(_costEffect)) return true;
            //
            // // TODO: Add a tag to indicate that the cost failed
            // Debug.Log($"Not enough {_costEffect.EffectDetails.Modifiers[0].Attribute.name} to cast this ability");
            // NotEnoughResourcesToCast?.Invoke();
            return false;
        }

        public void ApplyCost()
        {
            if (_costEffect == null) return;

            ApplyGameplayEffectToOwner(_costEffect);
        }


        public void Active(CharacterBehaviourBase target)
        {
            _target = target;
            _targetSystem = target.GameplayAbilitySystem;

            TryActiveAbility();
        }

        protected override IEnumerator InternalActiveAbility()
        {
            // Cost is optional
            if (_costEffect != null)
            {
                ApplyCost();
            }

            // TODO: REFACTOR GAS
            // CryptoQuestGameplayEffectSpec
            //     effectSpecSpec = (CryptoQuestGameplayEffectSpec)Owner.MakeOutgoingSpec(Effect);
            // effectSpecSpec.SetParameters(SkillDef.Info.SkillParameters);
            // _targetSystem.ApplyEffectSpecToSelf(effectSpecSpec);
            yield break;
        }
    }
}