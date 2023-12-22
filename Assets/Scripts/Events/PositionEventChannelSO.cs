using CryptoQuest.Map;
using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/Position Event Channel")]
    public class PositionEventChannelSO : GenericEventChannelSO<Vector3> { }
}