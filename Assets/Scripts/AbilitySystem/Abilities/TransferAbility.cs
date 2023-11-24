using System.Collections;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class TransferAbility : CastSkillAbility
    {
        [field: SerializeField] public VoidEventChannelSO TransferTriggerSucceeded { get; private set; }
        [field: SerializeField] public VoidEventChannelSO TransferCancel { get; private set; }

        protected override GameplayAbilitySpec CreateAbility() =>
            new TransferAbilitySpec(this);
    }

    public class TransferAbilitySpec : CastSkillAbilitySpec
    {
        private readonly TransferAbility _def;

        public TransferAbilitySpec(TransferAbility def) : base(def)
        {
            _def = def;
        }

        protected override IEnumerator OnAbilityActive()
        {
            if (CheckCastSkillSuccess() == false) yield break;

            RegisterBattleEndedEvents();

            ExecuteAbility();

            EndAbility();
        }

        protected override void InternalExecute(AbilitySystemBehaviour target)
        {
            _def.TransferTriggerSucceeded.EventRaised += OnTransferTriggerSucceeded;
            _def.TransferCancel.EventRaised += OnTransferCancel;
            ActionDispatcher.Dispatch(new TriggerTownTransferAbilityEvent());
        }

        private void OnTransferTriggerSucceeded()
        {
            _def.TransferTriggerSucceeded.EventRaised -= OnTransferTriggerSucceeded;
            _def.TransferCancel.EventRaised -= OnTransferCancel;
            ApplyCost();
        }

        private void OnTransferCancel()
        {
            _def.TransferTriggerSucceeded.EventRaised -= OnTransferTriggerSucceeded;
            _def.TransferCancel.EventRaised -= OnTransferCancel;
        }
    }
}