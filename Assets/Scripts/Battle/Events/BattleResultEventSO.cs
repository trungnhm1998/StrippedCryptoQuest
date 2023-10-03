using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Battle.Events
{
    [UnityEngine.CreateAssetMenu(fileName = "BattleResultEvent", menuName = "CryptoQuest/Events/Battle Result Event")]
    public class BattleResultEventSO : GenericEventChannelSO<BattleResultInfo> { }
}