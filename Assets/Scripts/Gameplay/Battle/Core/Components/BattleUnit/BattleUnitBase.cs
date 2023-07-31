using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class BattleUnitBase : MonoBehaviour, IBattleUnit
    {
        public AbilitySystemBehaviour Owner { get; set; }
        public BattleTeam OpponentTeam { get; set; }
        public BattleTeam OwnerTeam { get; set; }
        public bool IsDead => _isDead;
        public virtual AbstractAbility NormalAttack { get; protected set; }

        [SerializeField] protected AttributeScriptableObject _hpAttribute;

        [field: SerializeField]
        public CharacterDataSO UnitData { get; set; }

        [Header("Raise Events")]
        [SerializeField] protected VoidEventChannelSO _doneActionEventChannel;

        [Header("Listen Events")]
        [SerializeField] protected VoidEventChannelSO _doneShowDialogEvent;

        public AbstractAbility SelectedSkill { get; protected set; }

        public List<AbilitySystemBehaviour> TargetContainer { get; protected set; } = new();

        protected bool _isDead;
        protected bool _isPerformingAction;
        protected bool _isDoneShowAction;

        public virtual void Init(BattleTeam team, AbilitySystemBehaviour owner)
        {
            OwnerTeam = team;
            Owner = owner;
            ResetUnit();
        }

        protected virtual void OnEnable()
        {
            _hpAttribute.ValueChangeEvent += OnHPChanged;
            _doneShowDialogEvent.EventRaised += DoneShowAction;
        }

        protected virtual void OnDisable()
        {
            _hpAttribute.ValueChangeEvent -= OnHPChanged;
            _doneShowDialogEvent.EventRaised -= DoneShowAction;
        }

        private void DoneShowAction()
        {
            if (!_isPerformingAction) return;
            _isDoneShowAction = true;
        }

        public virtual void SetOpponentTeams(BattleTeam opponentTeam)
        {
            OpponentTeam = opponentTeam;
        }

        protected virtual void SetDefaultTarget()
        {
            if (OpponentTeam == null || OpponentTeam.Members.Count <= 0) return;

            if (TargetContainer.Count > 0) return;

            TargetContainer.Add(OpponentTeam.Members[0]);
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

        public virtual void SelectTargets(List<AbilitySystemBehaviour> targets)
        {
            TargetContainer.Clear();
            TargetContainer.AddRange(targets);
        }

        public virtual void SelectAllOpponent()
        {
            TargetContainer.Clear();
            TargetContainer.AddRange(OpponentTeam.Members);
        }

        public virtual void SelectAllAlly()
        {
            TargetContainer.Clear();
            TargetContainer.AddRange(OwnerTeam.Members);
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
            _isPerformingAction = true;
            Owner.TryActiveAbility(SelectedSkill);
            _doneActionEventChannel.RaiseEvent();
            yield return WaitUntilDoneShowAction();
            _isPerformingAction = false;
        }

        private IEnumerator WaitUntilDoneShowAction()
        {
            yield return new WaitWhile(() => !_isDoneShowAction && CanAction());
        }

        private bool CanAction()
        {
            // TODO: Check system unable to action tag here
            return !_isDead;
        }

        public virtual IEnumerator Resolve()
        {
            ResetUnit();
            yield return null;
        }

        public void ResetUnit()
        {
            SelectedSkill = null;
            TargetContainer.Clear();
            _isDoneShowAction = false;
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
            return TargetContainer.Count <= 0;
        }
    }
}