using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Events
{
    [CreateAssetMenu(fileName = "BattleEvent", menuName = "Crypto Quest/Battle/Events/Battle Event", order = 0)]
    public class BattleEventSO : GenericEventChannelSO<CompletedContext> { }
}