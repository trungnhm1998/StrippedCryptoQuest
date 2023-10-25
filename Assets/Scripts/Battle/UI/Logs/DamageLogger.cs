using System;
using CryptoQuest.AbilitySystem.Executions;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class DamageLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _damageMessage;
        [SerializeField] private LocalizedString _noDamageMessage;
        [SerializeField] private DealingDamageEvent _receivedDamageEvent;
        private TinyMessageSubscriptionToken _roundStartedEvent;

        private void Awake() => _receivedDamageEvent.DamageDealt += LogDamage;

        private void OnDestroy() => _receivedDamageEvent.DamageDealt -= LogDamage;

        private void LogDamage(Components.Character dealer, Components.Character receiver, float damage)
        {
            LocalizedString msg;

            if (Mathf.Approximately(damage, 0f))
                msg =
                    new LocalizedString(_noDamageMessage.TableReference, _noDamageMessage.TableEntryReference);
            else
            {
                msg = new LocalizedString(_damageMessage.TableReference, _damageMessage.TableEntryReference)
                {
                    {
                        Constants.DAMAGE_NUMBER, new FloatVariable()
                        {
                            Value = Math.Abs(damage)
                        }
                    }
                };
            }

            msg.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = receiver.DisplayName
            });

            Logger.QueueLog(msg);
        }
    }
}