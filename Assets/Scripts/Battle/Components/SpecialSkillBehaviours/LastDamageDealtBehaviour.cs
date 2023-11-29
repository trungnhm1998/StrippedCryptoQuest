using CryptoQuest.AbilitySystem.Executions;
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

        public float LastDamageDealt { get; private set; }
        public Character DamageReceiver { get; private set; }

        private void Awake()
        {
            _dealtDamageEvent.DamageDealt += SetLastDamageDealt;
        }

        private void OnDestroy()
        {
            _dealtDamageEvent.DamageDealt -= SetLastDamageDealt;
        }

        private void SetLastDamageDealt(Character dealer, Character receiver, float damage)
        {
            if (dealer != Character) return;
            LastDamageDealt = damage;
            DamageReceiver = receiver;
        }
    }
}