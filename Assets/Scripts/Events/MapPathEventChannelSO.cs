using CryptoQuest.Map;
using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/Map Path Event Channel")]
    public class MapPathEventChannelSO : GenericEventChannelSO<MapPathSO> { }
}