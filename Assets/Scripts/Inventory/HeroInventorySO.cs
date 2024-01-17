using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class HeroInventorySO : ScriptableObject
    {
        [field: SerializeField] public List<HeroSpec> OwnedHeroes { get; set; } = new();

        public void Remove(HeroSpec character)
        {
            for (var index = OwnedHeroes.Count - 1; index >= 0; index--)
            {
                var hero = OwnedHeroes[index];
                if (hero.Id != character.Id) continue;
                OwnedHeroes.RemoveAt(index);
            }
        }

        public void Add(HeroSpec character)
        {
            OwnedHeroes.Add(character);
        }
    }
}