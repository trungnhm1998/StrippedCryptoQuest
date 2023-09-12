using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Core/Events/String Event Channel")]
    public class StringEventChannelSO : GenericEventChannelSO<string> { }
}