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
        [SerializeField] protected AttributeScriptableObject _hpAttribute;

        [Header("Raise Events")]
        [SerializeField] protected VoidEventChannelSO _doneActionEventChannel;

        [Header("Listen Events")]
        [SerializeField] protected VoidEventChannelSO _doneShowDialogEvent;

        [field: SerializeField] public CharacterDataSO UnitData { get; set; }
        [field: SerializeField] public BattleUnitTagConfigSO TagConfig { get; private set; }

        public AbilitySystemBehaviour Owner { get; set; }
        public BattleTeam OpponentTeam { get; set; }
        public BattleTeam OwnerTeam { get; set; }
        public bool IsDead => _isDead;
        public CharacterInformation UnitInfo { get; private set; }
        public BaseBattleUnitLogic UnitLogic { get; private set; }

        protected bool _isDead;
        protected bool _isPerformingAction;
        protected bool _isDoneShowAction;

        public virtual void Init(BattleTeam team, AbilitySystemBehaviour owner)
        {
            OwnerTeam = team;
            Owner = owner;
            CreateCharacterInfo();
            InitBattleLogic();
            ResetUnit();
        }

        public void CreateCharacterInfo()
        {
            UnitInfo ??= UnitData.CreateCharacterInfo();
            UnitInfo.Owner = Owner;
        }

        protected virtual void InitBattleLogic()
        {
            UnitLogic = new BaseBattleUnitLogic(this, TagConfig);
            UnitLogic.Init();
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

            UnitLogic.SelectSingleTarget(OpponentTeam.Members[0]);
        }

        public virtual void SelectSingleTarget(AbilitySystemBehaviour target)
        {
            UnitLogic.SelectSingleTarget(target);
        }

        public virtual void SelectTargets(params AbilitySystemBehaviour[] targets)
        {
            UnitLogic.SelectTargets(targets);
        }

        public virtual void SelectTargets(List<AbilitySystemBehaviour> targets)
        {
            UnitLogic.SelectTargets(targets);
        }

        public virtual void SelectAllOpponent()
        {
            UnitLogic.SelectTargets(OpponentTeam.GetAvailableMembers());
        }

        public virtual void SelectAllAlly()
        {
            UnitLogic.SelectTargets(OwnerTeam.GetAvailableMembers());
        }

        public void SelectAbility(AbstractAbility ability)
        {
            UnitLogic.SelectedAbility = ability;
        }

        public virtual IEnumerator Prepare()
        {
            yield return new WaitUntil(UnitLogic.IsSelectedAbility);
            yield return new WaitUntil(UnitLogic.IsSelectedTarget);
        }

        public virtual IEnumerator Execute()
        {
            _isPerformingAction = true;
            UnitLogic.PerformUnitAction();

            //TODO: Too complicated to understand, can it be done using something like BattleManager.NextUnitAction?
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
            UnitLogic.Reset();
            _isDoneShowAction = false;
        }

        private void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (Owner == null || args.System != Owner.AttributeSystem) return;

            Owner.AttributeSystem.GetAttributeValue(_hpAttribute, out var attributValue);
            if (attributValue.CurrentValue > 0 || _isDead) return;

            _isDead = true;
            OwnerTeam.RemoveUnit(this);
            gameObject.SetActive(false);
        }

        public virtual void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}