using UnityEngine;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleUnitBase : MonoBehaviour, IBattleUnit
    {
        public AbilitySystemBehaviour Owner {get; set;}
        public BattleTeam OpponentTeam {get; set;}
        public BattleTeam OwnerTeam {get; set;}
        public virtual string OriginalName => "BattleUnit";
        public bool IsDead => _isDead;

        [SerializeField] protected AttributeScriptableObject _hpAttribute;

        [field: SerializeField]
        public TargetContainterSO TargetContainer { get; private set; }

        public AbstractAbility SelectedSkill { get; protected set; }

        protected List<string> _executeLogs = new();
        public List<string> ExecuteLogs => _executeLogs;

        protected BattleManager _battleManager;
        protected bool _isDead;

        public void Init(BattleTeam team, AbilitySystemBehaviour owner)
        {
            TargetContainer.Targets.Clear();
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

        protected virtual void SetDefaultTarget()
        {
            if (OpponentTeam == null || OpponentTeam.Members.Count <= 0) return;

            var currrentTargets = TargetContainer.Targets;
            if (currrentTargets.Count > 0) return;

            TargetContainer.SetSingleTarget(OpponentTeam.Members[0]);
        }

        public virtual void SelectSingleTarget(AbilitySystemBehaviour target)
        {
            if (OpponentTeam.Members.FindIndex(x => x == target) < 0) return;
            TargetContainer.SetSingleTarget(target);
        }

        public virtual void SelectAllTarget()
        {
            TargetContainer.SetMultipleTargets(OpponentTeam.Members);
        }

        public void SelectSkill(AbstractAbility selectedSkill)
        {
            SelectedSkill = selectedSkill;
        }

        public virtual IEnumerator Prepare()
        {
            yield return new WaitWhile(() => SelectedSkill == null);
            yield return new WaitWhile(HasNoTarget);
        }

        public virtual IEnumerator Execute()
        {
            Owner.TryActiveAbility(SelectedSkill);
            yield return null;
        }
        
        public virtual IEnumerator Resolve()
        {
            SelectedSkill = null;
            TargetContainer.Targets.Clear();
            yield return null;
        }

        private void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (Owner == null || args.System != Owner.AttributeSystem) return;

            Owner.AttributeSystem.GetAttributeValue(_hpAttribute, out AttributeValue attributValue);
            if (attributValue.CurrentValue > 0 || _isDead) return;

            _isDead = true;
            OwnerTeam.RemoveUnit(this);
            gameObject.SetActive(false);
        }

        public virtual void OnDeath()
        {
            Destroy(gameObject);
        }

        private bool HasNoTarget()
        {
            return TargetContainer.Targets.Count <= 0;
        }
    }
}