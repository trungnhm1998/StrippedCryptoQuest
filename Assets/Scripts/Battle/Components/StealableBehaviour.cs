using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components.SpecialSkillBehaviours;
using CryptoQuest.Character.Enemy;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class StealableBehaviour : CharacterComponentBase, IStealable
    {
        private List<Drop> _stealableDrops = new();

        public override void Init()
        {
            var provider = GetComponent<IDropsProvider>();
            _stealableDrops = provider.GetDrops().Where(d => d.Loot.IsItem).ToList();
        }

        public bool TrySteal(out LootInfo loot)
        {
            loot = null;
            Drop drop = null;
            foreach (var d in _stealableDrops)
            {
                var value = Random.value;
                Debug.Log($"Steal {d} chance: {d.Chance}, rolled: {value}");
                if (d.Chance < value) continue;
                drop = d;
                break;
            }

            if (drop == null) return false;
            _stealableDrops.Remove(drop);
            loot = drop.GetLoot();
            return true;
        }
    }
}