using CryptoQuest.Item;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Consume Item Event Channel")]
    public class ConsumableEventChannel : GenericEventChannelSO<ConsumableInfo> { }
}