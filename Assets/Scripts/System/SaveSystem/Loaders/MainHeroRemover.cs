using System;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class MainHeroRemover : Loader
    {
        [SerializeField] private HeroInventorySO _inventory;
        [SerializeField] private PartySO _party;

        public override void Load() => RemoveMainHeroFromInventory();

        private void RemoveMainHeroFromInventory()
        {
            for (var index = 0; index < _inventory.OwnedHeroes.Count; index++)
            {
                var hero = _inventory.OwnedHeroes[index];
                if (hero.Origin != null) continue;
                for (var i = 0; i < _party.Count; i++)
                {
                    var partySlot = _party[i];
                    if (partySlot.Hero.Id != 0) continue;
                    partySlot.Hero.Id = hero.Id;
                    partySlot.Hero.Experience = hero.Experience;
                    break;
                }

                _inventory.OwnedHeroes.RemoveAt(index);
                _party.SetParty(_party.GetParty());
                return;
            }
        }
    }
}