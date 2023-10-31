using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;
using CryptoQuest.Battle.Events;

namespace CryptoQuest.Battle.Components.SpecialSkillBehaviours
{
    public interface IStealerBehaviour
    {
        void StealTarget(Character target);
        void Steal(StealableInfo stealable);
    }

    public class HeroStealEnemyBehaviour : CharacterComponentBase, IStealerBehaviour
    {
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;
        private Character _stealTarget;

        public override void Init() { }

        public void StealTarget(Character target)
        {
            _stealTarget = target;

            var stealable = target.GetComponent<IStealable>();
            if (stealable == null)
            {
                Debug.LogWarning($"HeroStealEnemyBehaviour::This {target} is not stealable");
                return;
            }

            if (!stealable.TryGiveItems(this))
            {
                Debug.LogWarning($"HeroStealEnemyBehaviour::Steal from {target.DisplayName} failed!");
                OnStealFailed(Character, target);
                return;
            }

            return;
        }

        public void Steal(StealableInfo stealableItem)
        {
            _addLootRequestEventChannel.RaiseEvent(stealableItem.GetLoot());
            BattleEventBus.RaiseEvent(new StealSuccessEvent(Character, _stealTarget, stealableItem));
        }

        public void OnStealFailed(Character stealer, Character target)
        {
            BattleEventBus.RaiseEvent(new StealFailedEvent(stealer, target));
        }
    }
}