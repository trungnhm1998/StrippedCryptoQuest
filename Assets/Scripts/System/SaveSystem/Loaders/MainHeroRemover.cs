using System;
using CryptoQuest.Inventory;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class MainHeroRemover : Loader
    {
        [SerializeField] private HeroInventorySO _inventory;

        public override void Load() => RemoveMainHeroFromInventory();

        private void RemoveMainHeroFromInventory()
        {
            for (var index = 0; index < _inventory.OwnedHeroes.Count; index++)
            {
                var hero = _inventory.OwnedHeroes[index];
                if (hero.Origin != null) continue;
                _inventory.OwnedHeroes.RemoveAt(index);
                return;
            }
        }
    }
}