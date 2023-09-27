using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Events
{
    [CreateAssetMenu(fileName = "BattleEvent", menuName = "CryptoQuest/Battle/Events/Battle Event", order = 0)]
    public class BattleEventSO : GenericEventChannelSO<BattleContext> { }
}