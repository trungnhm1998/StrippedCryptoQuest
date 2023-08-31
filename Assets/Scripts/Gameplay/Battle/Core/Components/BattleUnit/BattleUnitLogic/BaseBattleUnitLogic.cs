using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Commands;
using CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.Gameplay.Battle.Helper;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class BaseBattleUnitLogic
    {
        public GameplayAbilitySpec SelectedAbility { get; set; }
        public List<AbilitySystemBehaviour> TargetContainer { get; protected set; } = new();
        public GameplayAbilitySpec NormalAttack { get; protected set; }
        public GameplayAbilitySpec RetreatAbility { get; protected set; }
        public GameplayAbilitySpec GuardAbility { get; protected set; }

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
            if (_battleUnit.IsDead) return true;
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
            // TODO: REFACTOR BATTLE
            //TODO: Apply abnormal status and check disable
            // Change to this because system can use ability from item
            // and _owner.TryActiveAbility can only activate skill inside system
            // if (_battleUnit.IsDead || !IsTargetsValid()) return;
            //
            // if (!(SelectedAbility is SimpleGameplayAbilitySpec ability)) return;
            // var useAbilityCommand = new UseAbilityCommand(
            //     _battleUnit,
            //     ability
            // );
            // BattleCommandHandler.OnReceivedCommand?.Invoke(useAbilityCommand);
            // SelectedAbility.ActivateAbility();
        }

        private bool IsTargetsValid()
        {
            foreach (var target in TargetContainer)
            {
                //TODO: target container contain battle unit might be better
                target.TryGetComponent<IBattleUnit>(out var unit);
                if (unit != null && unit.IsDead) return false;
            }

            return true;
        }

        public IEnumerable<SimpleGameplayAbilitySpec> GetActivableAbilities()
        {
            foreach (var ability in _owner.GetAbilitiesInBattle())
            {
                var abilitySO = ability.AbilitySO;
                var isSkillNotActivable = _tagConfig.CheckNotActivableSkillTag(abilitySO.Tags.AbilityTag);
                if (isSkillNotActivable) continue;
                yield return ability;
            }
        }
    }
}