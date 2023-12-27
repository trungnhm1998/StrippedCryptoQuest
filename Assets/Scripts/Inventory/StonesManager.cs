using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class StonesManager : MonoBehaviour
    {
        [SerializeField] private MagicStoneInventory _stoneInventory;
        private TinyMessageSubscriptionToken _addStoneEvent;
        private TinyMessageSubscriptionToken _removeStoneEvent;

        private void OnEnable()
        {
            _addStoneEvent = ActionDispatcher.Bind<AddStoneAction>(AddEquipment);
            _removeStoneEvent = ActionDispatcher.Bind<RemoveStoneAction>(RemoveEquipment);
        }

        private void AddEquipment(AddStoneAction ctx)
        {
            _stoneInventory.MagicStones.Add(ctx.Stone);
        }

        private void RemoveEquipment(RemoveStoneAction ctx)
        {
            _stoneInventory.MagicStones.Remove(ctx.Stone);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_addStoneEvent);
            ActionDispatcher.Unbind(_removeStoneEvent);
        }
    }
}