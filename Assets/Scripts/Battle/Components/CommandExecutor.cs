using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Commands;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public interface ICommandExecutor { }

    public class CommandExecutor : CharacterComponentBase, ICommandExecutor
    {
        private ICommand _command;

        public ICommand Command
        {
            get => _command;
            protected set => _command = value;
        }

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public IEnumerator ExecuteCommand()
        {
            // this character could die during presentation phase
            if (Character.IsValidAndAlive() == false) yield break;
            yield return OnPreExecuteCommand();
            yield return _command.Execute(); // this should not be null
            yield return OnPostExecuteCommand();
        }

        protected virtual IEnumerator OnPostExecuteCommand()
        {
            _command = NullCommand.Instance;
            yield return new WaitForSeconds(1f);
        }

        protected virtual IEnumerator OnPreExecuteCommand()
        {
            yield return new WaitForSeconds(1f);
        }

        public override void Init()
        {
            _command = NullCommand.Instance;
        }

        public event Action OnTurnStarted;

        /// <summary>
        /// Update turn of effect turn base, apply effect present it
        /// </summary>
        public void PreTurn()
        {
            _cache.Clear();
            Character.GameplayEffectSystem.UpdateAttributeModifiersUsingAppliedEffects();
            Character.AbilitySystem.AttributeSystem.PostAttributeChange += CacheChange;
            OnTurnStarted?.Invoke(); // this will cause turn base effect to update
            Character.AbilitySystem.AttributeSystem.PostAttributeChange -= CacheChange;
            LogChanges();
        }

        /// <summary>
        /// Currently for only debug purpose
        /// </summary>
        private void LogChanges()
        {
            while (_cache.Count > 0)
            {
                var info = _cache.Dequeue();
                var attribute = info.Attribute;
                var oldValue = info.OldValue;
                var newValue = info.NewValue;
                var delta = newValue.CurrentValue - oldValue.CurrentValue;
                var deltaString = delta > 0 ? $"+{delta}" : $"{delta}";
                var log = $"{attribute.name} {deltaString}";
                Debug.Log(log);
            }
        }

        struct Info
        {
            public AttributeScriptableObject Attribute;
            public AttributeValue OldValue;
            public AttributeValue NewValue;
        }

        private readonly Queue<Info> _cache = new();

        private void CacheChange(AttributeScriptableObject attribute, AttributeValue oldValue,
            AttributeValue newValue)
        {
            var info = new Info
            {
                Attribute = attribute,
                OldValue = oldValue,
                NewValue = newValue
            };
            _cache.Enqueue(info);
        }

        /// <summary>
        /// Remove any effect that expired at the beginning of the turn
        /// </summary>
        public IEnumerator PostTurn()
        {
            yield break;
        }
    }
}