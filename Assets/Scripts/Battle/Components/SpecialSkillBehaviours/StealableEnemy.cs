using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;
using System.Linq;
using CryptoQuest.Character.Enemy;

namespace CryptoQuest.Battle.Components.SpecialSkillBehaviours
{
    public interface IStealable
    {
        bool TryGiveItems(IStealerBehaviour stealer);
    }

    public class StealableEnemy : CharacterComponentBase, IStealable
    {
        private EnemyBehaviour _enemyBehaviour;
        private List<StealableInfo> _cloneStealables;

        public override void Init()
        {
            _enemyBehaviour = GetComponent<EnemyBehaviour>(); 
            _cloneStealables = _enemyBehaviour.Def.StealableInfos.ToList();
        }

        public bool TryGiveItems(IStealerBehaviour stealer)
        {
            if (_cloneStealables.Count <= 0) return false;

            var stealSussess = false;

            for (int index = 0; index < _cloneStealables.Count;)
            {
                var stealable = _cloneStealables[index];
                if (stealable.TryGiveLoot(stealer))
                {
                    stealSussess = true;
                    _cloneStealables.Remove(stealable);
                    continue;
                }

                index++;
            }

            return stealSussess;
        }
    }
}