using CryptoQuest.Battle.Events;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;

namespace CryptoQuest.Battle.Components.SpecialSkillBehaviours
{
    public class StealerBehaviour : CharacterComponentBase, IStealerBehaviour
    {
        [SerializeField] private LootEventChannelSO _requestAddLootEventChannel;

        public void Steal(Character target)
        {
            var stealable = target.GetComponent<IStealable>();
            if (stealable.TrySteal(out var loot))
                OnStealSucceed(target, loot);
            else
                OnStealFailed(target);
        }

        private void OnStealSucceed(Character target, LootInfo loot)
        {
            _requestAddLootEventChannel.RaiseEvent(loot);
            BattleEventBus.RaiseEvent(new StealSuccessEvent(Character, target, loot));
        }

        private void OnStealFailed(Character target)
        {
            BattleEventBus.RaiseEvent(new StealFailedEvent(Character, target));
        }
    }
}