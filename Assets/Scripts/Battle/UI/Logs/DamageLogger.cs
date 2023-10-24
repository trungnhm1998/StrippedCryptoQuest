using System;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.Character;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
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
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;
        private TinyMessageSubscriptionToken _roundStartedEvent;

        private void Awake()
        {
            _roundStartedEvent = BattleEventBus.SubscribeEvent<LoadedEvent>(RegisterEvents);
        }

        private void RegisterEvents(LoadedEvent _)
        {
            _attributeChangeEvent.Changed += Notify;
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_roundStartedEvent);
            _attributeChangeEvent.Changed -= Notify;
        }

        /// <summary>
        /// To handle Damage Overtime
        /// </summary>
        private void Notify(AttributeSystemBehaviour owner, AttributeValue oldVal, AttributeValue newVal)
        {
            if (newVal.Attribute != AttributeSets.Health) return;
            var damage = newVal.CurrentValue - oldVal.CurrentValue;
            LogDamage(owner.GetComponent<Components.Character>(), damage);
        }

        private void LogDamage(Components.Character character, float damage)
        {
            LocalizedString msg;
            // Escape early here because damage > 0 equal healing
            if (damage > 0) return;

            if (Mathf.Approximately(damage, 0f))
                msg =
                    new LocalizedString(_noDamageMessage.TableReference, _noDamageMessage.TableEntryReference);
            else
            {
                character.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hp);
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
                Value = character.DisplayName
            });

            Logger.QueueLog(msg);
        }
    }
}