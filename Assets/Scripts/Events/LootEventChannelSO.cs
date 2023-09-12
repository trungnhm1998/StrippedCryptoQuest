using CryptoQuest.Gameplay.Loot;
using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/Loot Event Channel", fileName = "LootEventChannel")]
    public class LootEventChannelSO : GenericEventChannelSO<LootInfo> { }
}