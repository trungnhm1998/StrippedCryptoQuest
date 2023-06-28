using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Indigames.AbilitySystem.Sample
{
    public class BattleUnitBase : MonoBehaviour, IBattleUnit
    {
        public AbilitySystemBehaviour Owner {get; set;}
        public List<AbilitySystemBehaviour> Targets {get; set;}
        public List<AbilitySystemBehaviour> OwnerTeam {get; set;}

        [SerializeField] protected AttributeScriptableObject _hpAttribute;

        [SerializeField] private TargetContainterSO _targetContainer;
        public TargetContainterSO TargetContainer => _targetContainer;

        protected BattleManager _battleManager;
        protected bool _isPerformSkillThisTurn;
        protected bool _isDeath;

        public void Init(BattleManager manager, AbilitySystemBehaviour owner)
        {
            _targetContainer.Targets.Clear();
            _battleManager = manager;
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

        public virtual void SetTeams(ref List<AbilitySystemBehaviour> ownerTeam, ref List<AbilitySystemBehaviour> targets)
        {
            Targets = targets;
            OwnerTeam = ownerTeam;
            SetDefaultTarget();
        }

        protected virtual void SetDefaultTarget()
        {
            if (Targets == null || Targets.Count <= 0) return;

            var currrentTargets = TargetContainer.Targets;
            if (currrentTargets.Count > 0) return;

            _targetContainer.SetSingleTarget(Targets[0]);
            Debug.Log($"{Targets[0].gameObject}");
        }

        public virtual void SelectSingleTarget(AbilitySystemBehaviour target)
        {
            if (Targets.FindIndex(x => x == target) < 0) return;
            _targetContainer.SetSingleTarget(target);
        }

        public virtual void SelectAllTarget()
        {
            _targetContainer.SetMultipleTargets(Targets);
        }

        public virtual IEnumerator Execute()
        {
            _isPerformSkillThisTurn = false;
            yield return null;
        }
        
        public virtual IEnumerator Resolve()
        {
            ClearDeathTargets();
            SetDefaultTarget();
            yield return null;
        }

        private void ClearDeathTargets()
        {
            var currrentTargets = TargetContainer.Targets;
            for (int i = 0; i < currrentTargets.Count; i++)
            {
                var target = currrentTargets[i];
                if (target != null && target.gameObject != null) continue;

                TargetContainer.Targets.Remove(target);
                i--;
            }
        }

        private void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (Owner == null || args.System != Owner.AttributeSystem) return;
            Owner.AttributeSystem.GetAttributeValue(_hpAttribute, out var attributValue);
            if (attributValue.CurrentValue > 0 || _isDeath) return;
            _isDeath = true;
            _battleManager.RemoveUnit(this);
        }

        public virtual void OnDeath()
        {
            OwnerTeam.Remove(Owner);
            Destroy(gameObject);
        }

        public virtual void OnPerformSkill()
        {
            _isPerformSkillThisTurn = true;
        }
    }
}