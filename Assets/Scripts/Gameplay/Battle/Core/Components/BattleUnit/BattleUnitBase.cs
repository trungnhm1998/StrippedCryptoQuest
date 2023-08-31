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
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands;
using CryptoQuest.Gameplay.Battle.Core.Commands;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    [Obsolete]
    public class BattleUnitBase : MonoBehaviour, IBattleUnit
    {
        [SerializeField] protected AttributeScriptableObject _hpAttribute;

        [Header("Raise Events")]
        [SerializeField] protected VoidEventChannelSO _showNextMarkEventChannel;

        [Header("Listen Events")]
        [SerializeField] protected VoidEventChannelSO _doneShowDialogEvent;

        [field: SerializeField] public CharacterDataSO UnitData { get; set; }
        [field: SerializeField] public BattleUnitTagConfigSO TagConfig { get; private set; }

        public AbilitySystemBehaviour Owner { get; set; }
        public BattleTeam OpponentTeam { get; set; }
        public BattleTeam OwnerTeam { get; set; }
        public bool IsDead => _isDead;
        public CharacterInformation UnitInfo { get; private set; }
        public BaseBattleUnitLogic UnitLogic { get; protected set; }

        protected bool _isDead;
        private FinishTurnCommand _finishTurnCommand;

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
            if (Owner == null) return;
            UnitInfo.Owner = Owner;
            Owner.AttributeSystem.PostAttributeChange += OnHPChanged;
        }

        protected virtual void InitBattleLogic()
        {
            UnitLogic = new BaseBattleUnitLogic(this, TagConfig);
            UnitLogic.Init();
        }

        protected virtual void OnDisable()
        {
            if (Owner != null)
            {
                Owner.AttributeSystem.PostAttributeChange -= OnHPChanged;
            }
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
        
        public void SelectAbility(GameplayAbilitySpec gameplayAbilitySpec)
        {
            UnitLogic.SelectedAbility = gameplayAbilitySpec;
        }

        public virtual IEnumerator Prepare()
        {
            if (UnitLogic.IsUnableAction()) yield break;
            yield return new WaitUntil(UnitLogic.IsSelectedAbility);
            yield return new WaitUntil(UnitLogic.IsSelectedTarget);
        }

        public virtual IEnumerator Execute()
        {
            UnitLogic.PerformUnitAction();
            FinishTurn();
            yield break;
        }

        private void FinishTurn()
        {
            _finishTurnCommand ??= new FinishTurnCommand();
            _finishTurnCommand.ShowNextMarkEvent = _showNextMarkEventChannel.RaiseEvent;
            _finishTurnCommand.DoneShowDialogEvent = _doneShowDialogEvent;
            BattleCommandHandler.OnReceivedCommand?.Invoke(_finishTurnCommand);
        }

        private bool CanAction()
        {
            // TODO: Check system unable to action tag here
            return !_isDead && !UnitLogic.IsUnableAction();
        }

        public virtual IEnumerator Resolve()
        {
            ResetUnit();
            yield return null;
        }

        public void ResetUnit()
        {
            UnitLogic.Reset();
            CheckUnitDead();
        }

        private void OnHPChanged(AttributeScriptableObject attribute, AttributeValue oldValue,
            AttributeValue newValue)
        {
            if (attribute != _hpAttribute) return;
            if (oldValue.Attribute != _hpAttribute) return;
            CheckUnitDead();
        }

        private void CheckUnitDead()
        {
            Owner.AttributeSystem.TryGetAttributeValue(_hpAttribute, out var attributValue);
            if (attributValue.CurrentValue > 0 || _isDead) return;

            _isDead = true;
            OwnerTeam.RemoveUnit(this);
        }

        public virtual void OnDeath() { }
    }
}