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

        protected AbstractAbility _selectedSkill;
        public AbstractAbility SelectedSkill => _selectedSkill;

        protected BattleManager _battleManager;
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
        }

        protected virtual void SetDefaultTarget()
        {
            if (Targets == null || Targets.Count <= 0) return;

            var currrentTargets = TargetContainer.Targets;
            if (currrentTargets.Count > 0) return;

            _targetContainer.SetSingleTarget(Targets[0]);
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

        public void SelectSkill(AbstractAbility selectedSkill)
        {
            _selectedSkill = selectedSkill;
        }


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
            _battleManager.RemoveUnit(this);
        }

        public virtual void OnDeath()
        {
            OwnerTeam.Remove(Owner);
            Destroy(gameObject);
        }

        private bool HasNoTarget()
        {
            return TargetContainer.Targets.Count <= 0;
        }
    }
}