﻿using CryptoQuest.Character.Ability;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class NormalAttack : CharacterComponentBase
    {
        [SerializeField] private NormalAttackAbility _normalAttackAbility;

        private NormalAttackAbilitySpec _spec;
        public override void Init()
        {
            _spec = Character.AbilitySystem.GiveAbility<NormalAttackAbilitySpec>(_normalAttackAbility);
        }

        public void Attack(IDamageable damageable)
        {
            _spec.Execute(damageable);
        }
    }
}