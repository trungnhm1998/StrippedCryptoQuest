using System;
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
            if (damage < 0)
            {
                localizedMessage = _damageMessage;
                localizedMessage.Add(Constants.DAMAGE_NUMBER, new FloatVariable()
                {
                    Value = Math.Abs(damage)
                });
            }
            else
                localizedMessage = _noDamageMessage;

            Debug.Log($"{character.LocalizedName.GetLocalizedString()}: {damage}");
            localizedMessage.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = character.DisplayName
            });

            Logger.AppendLog(localizedMessage);
        }
    }
}