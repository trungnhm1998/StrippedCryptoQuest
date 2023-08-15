using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class BaseBattleUnitLogic
    {
        public AbstractAbility SelectedAbility { get; set; }
        public List<AbilitySystemBehaviour> TargetContainer { get; protected set; } = new();
        public AbstractAbility NormalAttack { get; protected set; }
        public AbstractAbility RetreatAbility { get; protected set; }
        public AbstractAbility GuardAbility { get; protected set; }

        protected IBattleUnit _battleUnit;
        protected AbilitySystemBehaviour _owner;
        protected CharacterDataSO _unitData;
        protected BattleUnitTagConfigSO _tagConfig;

        public BaseBattleUnitLogic(IBattleUnit unit, BattleUnitTagConfigSO tagConfig)
        {
            _battleUnit = unit;
            _owner = unit.Owner;
            _unitData = unit.UnitData;
            _tagConfig = tagConfig;
        }

        public virtual void Init()
        {
            GrantDefaultAbilities();
        }
        
        private void GrantDefaultAbilities()
        {
            NormalAttack = _owner.GiveAbility(_unitData.NormalAttack);

            if (_unitData.RetreatAbilitySO)
            {
                RetreatAbility = _owner.GiveAbility(_unitData.RetreatAbilitySO);
            }

            if (_unitData.GuardAbilitySO)
            {
                GuardAbility = _owner.GiveAbility(_unitData.GuardAbilitySO);
            }

            foreach (var ability in _unitData.GrantedAbilities)
            {
                _owner.GiveAbility(ability);
            }
        }

        public virtual void Reset()
        {
            SelectedAbility = null;
            TargetContainer.Clear();
        }

        public virtual bool IsSelectedAbility() => SelectedAbility != null;

        public virtual bool IsSelectedTarget() => TargetContainer.Count > 0;

        public virtual bool IsUnableAction()
        {
            foreach (var tag in _tagConfig.DisableActionTags)
            {
                if (_owner.TagSystem.GrantedTags.Contains(tag)) return true;
            }
            return false;
        }

        public virtual void SelectSingleTarget(AbilitySystemBehaviour target)
        {
            TargetContainer.Clear();
            TargetContainer.Add(target);
        }

        /// <summary>
        /// Use this to select group of units
        /// </summary>
        /// <param name="targets"></param>
        public virtual void SelectTargets(params AbilitySystemBehaviour[] targets)
        {
            TargetContainer.Clear();
            TargetContainer.AddRange(targets);
        }

        public virtual void SelectTargets(IEnumerable<AbilitySystemBehaviour> targets)
        {
            TargetContainer.Clear();
            TargetContainer.AddRange(targets);
        }

        public virtual void SelectAllOpponent()
        {
            SelectTargets(_battleUnit.OpponentTeam.GetAvailableMembers());
        }

        public virtual void SelectAllAlly()
        {
            SelectTargets(_battleUnit.OwnerTeam.GetAvailableMembers());
        }

        public virtual void PerformUnitAction()
        {
            //TODO: Apply abnormal status and check disable
            // Change to this because system can use ability from item
            // and _owner.TryActiveAbility can only activate skill inside system
            if (!IsTargetsValid()) return;
            SelectedAbility.ActivateAbility();
        }

        private bool IsTargetsValid()
        {
            foreach(var target in TargetContainer)
            {
                //TODO: target container contain battle unit might be better
                target.TryGetComponent<IBattleUnit>(out var unit);
                if (unit != null && unit.IsDead) return false;
            }
            return true;
        }

        public virtual void ActivateAbilityWithTag(TagScriptableObject tag)
        {
            var abilities = _owner.GrantedAbilities.Abilities;
            foreach (var ability in abilities)
            {
                if (ability.AbilitySO.Tags.AbilityTag != tag) continue;
                _owner.TryActiveAbility(ability);
            }
        }

        public IEnumerable<AbstractAbility> GetActivableAbilities()
        {
            foreach (var ability in _owner.GrantedAbilities.Abilities)
            {
                var abilitySO = ability.AbilitySO;
                var isAtivableSkill = _tagConfig.CheckNotActivableSkillTag(abilitySO.Tags.AbilityTag);
                if (abilitySO is not AbilitySO || isAtivableSkill) continue;
                yield return ability;
            }
        }
    }
}