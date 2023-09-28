using CryptoQuest.Quest.Authoring;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/Quest Event Channel", fileName = "QuestEventChannelSO")]
    public class QuestTriggerEventChannelSO : GenericEventChannelSO<QuestSO> { }
}