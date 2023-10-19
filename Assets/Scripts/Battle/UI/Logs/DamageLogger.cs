using System;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Executions;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class DamageLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _damageMessage;
        [SerializeField] private LocalizedString _noDamageMessage;
        [SerializeField] private NotifyDamage _notifyDamage;

        private void Awake()
        {
            _notifyDamage.DamageDealt += LogDamage;
        }

        private void OnDestroy()
        {
            _notifyDamage.DamageDealt -= LogDamage;
        }

        private void LogDamage(Components.Character character, float damage)
        {
            LocalizedString localizedMessage;
            // Escape early here because damage > 0 equal healing
            if (damage > 0) return;

            if (Mathf.Approximately(damage, 0f))
                localizedMessage = _noDamageMessage;
            else
            {
                character.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hp);
                damage = hp.CurrentValue - Math.Abs(damage) < 0 ? hp.CurrentValue : Math.Abs(damage);
                localizedMessage = _damageMessage;
                localizedMessage.Add(Constants.DAMAGE_NUMBER, new FloatVariable()
                {
                    Value = Math.Abs(damage)
                });
            }

            localizedMessage.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = character.DisplayName
            });

            Logger.AppendLog(localizedMessage);
        }
    }
}