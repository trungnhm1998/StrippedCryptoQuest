using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.Ability;
using CryptoQuest.Map;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Item.Ocarina
{
    public class OcarinaAbility : ConsumeItemAbility
    {
        public event Action<MapPathSO> TeleportToTown;

        // TODO: Load from save
        [field: SerializeField] public List<OcarinaEntrance> Locations { get; private set; }

        protected override GameplayAbilitySpec CreateAbility() => new OcarinaAbilitySpec(this);

        public void RegisterTown(OcarinaEntrance town)
        {
            if (!Locations.Contains(town))
                Locations.Add(town);

            // TODO: SAVE
        }

        /// <summary>
        /// Logic for Ocarina
        /// 
        /// Nested helps me call the private method, "friend" class
        /// </summary>
        public class OcarinaAbilitySpec : ConsumableAbilitySpec
        {
            private readonly OcarinaAbility _def;

            public OcarinaAbilitySpec(OcarinaAbility ocarinaAbility) : base(ocarinaAbility)
            {
                _def = ocarinaAbility;
            }

            protected override IEnumerator OnAbilityActive()
            {
                yield return null;
            }

            public void TeleportToTown(OcarinaEntrance town)
            {
                _def.TeleportToTown?.Invoke(town);
            }
        }
    }
}