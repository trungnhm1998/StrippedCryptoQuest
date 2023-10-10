using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEngine;

namespace CryptoQuest.Character.Ability
{
    /// <summary>
    /// https://docs.google.com/spreadsheets/d/1EDrXrT1if63TLam1km_dLx2VvHt2rks2OWXEUq5OEPg/edit#gid=2122798992
    /// Base data class for character skill
    /// </summary>
    public class CastSkillAbility : AbilityScriptableObject
    {
        [field: SerializeField] public GameplayEffectSO Effect { get; private set; }
        [field: SerializeField] public float SuccessRate { get; private set; } = 100;
        [field: SerializeField] public SkillInfo Parameters { get; private set; }
        [field: SerializeField] public SkillTargetType TargetType { get; private set; }

        protected override GameplayAbilitySpec CreateAbility() => new CastSkillAbilitySpec(this);
    }

    public class CastSkillAbilitySpec : GameplayAbilitySpec
    {
        private GameplayEffectDefinition _costEffect;
        private readonly CastSkillAbility _def;
        public CastSkillAbility Def => _def;

        public CastSkillAbilitySpec(CastSkillAbility def) => _def = def;

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);

            _costEffect = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            _costEffect.EffectAction = new InstantAction();
            _costEffect.EffectDetails = new EffectDetails()
                {
                    Modifiers = new EffectAttributeModifier[]
                    {
                        new()
                        {
                            Attribute = AttributeSets.Mana,
                            ModifierType = EAttributeModifierType.Add,
                            Value = -_def.Parameters.Cost
                        }
                    }
                };
        }

        public override bool CanActiveAbility()
        {
            return base.CanActiveAbility() && CheckCost() && CanCast();
        }

        /// <summary>
        /// GameplayAbility.cpp::CheckCost line 906
        /// Create a cost effect spec, use the modifier and calculate the remaining resources, if the remaining resources
        /// is greater than 0 apply the effect
        /// </summary>
        /// <returns>Return true if after subtracted attribute that the cost needs greater than 0</returns>
        private bool CheckCost()
        {
            if (_costEffect == null) return true;
            if (Owner == null) return true;
            if (Owner.CanApplyAttributeModifiers(_costEffect)) return true;

            Debug.Log($"Not enough {_costEffect.EffectDetails.Modifiers[0].Attribute.name} to cast this ability");
            BattleEventBus.RaiseEvent(new MpNotEnoughEvent());
            return false;
        }

        private bool CanCast()
        {
            var result = Random.Range(0, 100) < _def.SuccessRate;
            if (!result)
            {
                Debug.Log($"Failed to cast {_def.name}");
                BattleEventBus.RaiseEvent(new CastSkillFailedEvent());
            }

            return result;
        }

        private void ApplyCost()
        {
            if (_costEffect == null) return;

            ApplyGameplayEffectToOwner(_costEffect);
        }

        private AbilitySystemBehaviour[] _targets;

        public void Execute(params AbilitySystemBehaviour[] characters)
        {
            _targets = characters;
            TryActiveAbility();
        }

        protected override IEnumerator OnAbilityActive()
        {
            ApplyCost();

            var spec = CreateEffectSpec();
            foreach (var target in _targets)
            {
                if (IsTargetEvaded(target))
                {
                    BattleEventBus.RaiseEvent(new MissedEvent());
                    continue;
                }

                target.ApplyEffectSpecToSelf(spec);
            }

            yield break;
        }

        private EffectSpec CreateEffectSpec()
        {
            var clonedDef = Object.Instantiate(_def.Effect);
            return clonedDef.CreateEffectSpec(Owner, this);
        }

        private bool IsTargetEvaded(AbilitySystemBehaviour target)
        {
            var evadeBehaviour = target.GetComponent<IEvadable>();
            return evadeBehaviour != null && evadeBehaviour.TryEvade();
        }
    }
}