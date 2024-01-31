using System;
using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Inventory;
using CryptoQuest.System.SaveSystem;
using CryptoQuest.System.SaveSystem.Loaders;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    [Serializable]
    public struct PartyOrderSerializeObject
    {
        public int[] Ids;
    }

    [Serializable]
    public class PartyLoader : Loader
    {
        [SerializeField] private HeroInventorySO _heroInventory;
        [SerializeField] private PartySO _partySO;
        [SerializeField] private SaveSystemSO _saveSystemSO;
        [SerializeField] private UnitSO _defaultMainHero;

        public override void Load()
        {
            _partySO.SetParty(CreatePartyFromSave());
        }

        private PartySlotSpec[] CreatePartyFromSave()
        {
            if (!_saveSystemSO.SaveData.TryGetValue(_partySO.name, out var json))
                return CreateDefaultParty();

            var partyOrder = JsonUtility.FromJson<PartyOrderSerializeObject>(json).Ids;
            if (partyOrder.Length == 0)
                return CreateDefaultParty();
            var party = new List<PartySlotSpec>();
            for (var index = 0; index < partyOrder.Length; index++)
            {
                var id = partyOrder[index];
                if (TryGetHeroInInventory(id, out var hero) == false) continue;
                party.Add(new PartySlotSpec()
                {
                    Hero = hero
                });
            }

            return party.ToArray();
        }

        private bool TryGetHeroInInventory(int id, out HeroSpec outHero)
        {
            if (id == 0)
            {
                outHero = CreateMainHeroSpec();
                return true;
            }

            outHero = null;
            foreach (var hero in _heroInventory.OwnedHeroes)
            {
                if (hero.Id != id) continue;
                outHero = hero;
                return true;
            }

            return false;
        }

        /// <returns>Default party with single main hero at first slot</returns>
        private PartySlotSpec[] CreateDefaultParty()
        {
            _saveSystemSO.SaveData[_partySO.name] = JsonConvert.SerializeObject(new PartyOrderSerializeObject()
            {
                Ids = new[] { 0 }
            });
            return new[]
            {
                new PartySlotSpec()
                {
                    Hero = CreateMainHeroSpec()
                }
            };
        }

        private HeroSpec CreateMainHeroSpec()
        {
            return new HeroSpec()
            {
                Id = 0,
                Elemental = _defaultMainHero.Element,
                Class = _defaultMainHero.Class,
                Stats = _defaultMainHero.Stats,
                Origin = _defaultMainHero.Origin
            };
        }
    }
}