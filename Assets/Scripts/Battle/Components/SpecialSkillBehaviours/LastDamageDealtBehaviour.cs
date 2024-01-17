using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Components.SpecialSkillBehaviours
{
    /// <summary>
    /// Saving infomation about last time this character dealt damage
    /// for some special skill like absorb 
    /// </summary>
    public class LastDamageDealtBehaviour : CharacterComponentBase
    {
        [SerializeField] private DealingDamageEvent _dealtDamageEvent;
        private TinyMessageSubscriptionToken _turnEndedEvent;

        public float LastDamageDealt { get; private set; }
        public Character DamageReceiver { get; private set; }

        private void Awake()
        {
            _dealtDamageEvent.DamageDealt += SetLastDamageDealt;
            _turnEndedEvent = BattleEventBus.SubscribeEvent<TurnEndedEvent>(OnTurnEndedEvent);
        }

        private void OnDestroy()
        {
            _dealtDamageEvent.DamageDealt -= SetLastDamageDealt;
            BattleEventBus.UnsubscribeEvent(_turnEndedEvent);
        }

        private void OnTurnEndedEvent(TurnEndedEvent obj)
        {
            LastDamageDealt = 0;
            DamageReceiver = null;
        }

        private void SetLastDamageDealt(Character dealer, Character receiver, float damage)
        {
            if (dealer != Character) return;
            LastDamageDealt = damage;
            DamageReceiver = receiver;
        }
    }
}