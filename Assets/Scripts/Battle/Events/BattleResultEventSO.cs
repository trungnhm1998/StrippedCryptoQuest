using System;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Battle.Events
{
    [UnityEngine.CreateAssetMenu(fileName = "BattleResultEvent", menuName = "CryptoQuest/Events/Battle Result Event")]
    [Obsolete]
    public class BattleResultEventSO : GenericEventChannelSO<BattleResultInfo> { }
}