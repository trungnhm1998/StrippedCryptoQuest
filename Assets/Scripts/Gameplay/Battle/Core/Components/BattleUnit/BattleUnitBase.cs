using UnityEngine;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleUnitBase : MonoBehaviour, IBattleUnit
    {
        public AbilitySystemBehaviour Owner {get; set;}
        public BattleTeam OpponentTeam {get; set;}
        public BattleTeam OwnerTeam {get; set;}

        [SerializeField] protected AttributeScriptableObject _hpAttribute;

        [SerializeField] private TargetContainterSO _targetContainer;
        public TargetContainterSO TargetContainer => _targetContainer;

        protected AbstractAbility _selectedSkill;
        public AbstractAbility SelectedSkill => _selectedSkill;

        protected BattleManager _battleManager;
        protected bool _isDeath;

        public void Init(BattleTeam team, AbilitySystemBehaviour owner)
        {
            _targetContainer.Targets.Clear();
            OwnerTeam = team;
            Owner = owner;
        }

        protected virtual void OnEnable()
        {
            _hpAttribute.ValueChangeEvent += OnHPChanged;
        }

        protected virtual void OnDisable()
        {
            _hpAttribute.ValueChangeEvent -= OnHPChanged;
        }

        public virtual void SetOpponentTeams(BattleTeam opponentTeam)
        {
            OpponentTeam = opponentTeam;
        }

        public virtual AbilitySystemBehaviour GetOwner()
        {
            return Owner;
        } 

        protected virtual void SetDefaultTarget()
        {
            if (OpponentTeam == null || OpponentTeam.Members.Count <= 0) return;

            var currrentTargets = TargetContainer.Targets;
            if (currrentTargets.Count > 0) return;

            _targetContainer.SetSingleTarget(OpponentTeam.Members[0]);
        }

        public virtual void SelectSingleTarget(AbilitySystemBehaviour target)
        {
            if (OpponentTeam.Members.FindIndex(x => x == target) < 0) return;
            _targetContainer.SetSingleTarget(target);
        }

        public virtual void SelectAllTarget()
        {
            _targetContainer.SetMultipleTargets(OpponentTeam.Members);
        }

        public void SelectSkill(AbstractAbility selectedSkill)
        {
            _selectedSkill = selectedSkill;
        }

        public BattleTeam GetOpponent() => OpponentTeam;

        public virtual string GetOriginalName() => "";

        public virtual IEnumerator Prepare()
        {
            while (_selectedSkill == null)
            {
                yield return false;
            }

            
            while (HasNoTarget())
            {
                yield return false;
            }
        }
        public virtual IEnumerator Execute()
        {
            Owner.TryActiveAbility(_selectedSkill);
            yield return null;
        }
        
        public virtual IEnumerator Resolve()
        {
            _selectedSkill = null;
            TargetContainer.Targets.Clear();
            yield return null;
        }

        private void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (Owner == null || args.System != Owner.AttributeSystem) return;

            Owner.AttributeSystem.GetAttributeValue(_hpAttribute, out var attributValue);
            if (attributValue.CurrentValue > 0 || _isDeath) return;

            _isDeath = true;
            OwnerTeam.RemoveUnit(this);
        }

        public virtual void OnDeath()
        {
            gameObject.SetActive(false);
        }

        private bool HasNoTarget()
        {
            return TargetContainer.Targets.Count <= 0;
        }
    }
}