using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;
using CryptoQuest.Battle.Events;
using CryptoQuest.AbilitySystem.Executions;

namespace CryptoQuest.Battle.Components.SpecialSkillBehaviours
{
    /// <summary>
    /// Saving infomation about last time this character dealt damage
    /// for some special skill like absorb 
    /// </summary>
    public class LastDamageDealtBehaviour : CharacterComponentBase
    {
        [SerializeField] private DealingDamageEvent _dealtDamageEvent;

        public float LastDamageDealt { get; private set; }
        public Components.Character DamageReceiver { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _dealtDamageEvent.DamageDealt += SetLastDamageDealt;
        }

        private void OnDestroy()
        {
            _dealtDamageEvent.DamageDealt -= SetLastDamageDealt;
        }

        private void SetLastDamageDealt(Components.Character dealer, Components.Character receiver, float damage) 
        {
            if (dealer != Character) return;
            LastDamageDealt = damage;
            DamageReceiver = receiver;
        }

        public override void Init() { }
    }
}